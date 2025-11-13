using Core.App.Services;
using Core.App.Utils;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Errors;
using Core.QueryParams;
using Core.Repositories;
using Core.Services;
using Core.Shared.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuestionService : BaseService<Question>, IQuestionService
{
    private readonly IQuestionRepository _repository;
    private readonly IOptionRepository _optionRepository;

    private readonly ISubjectService _subjectService;
    private readonly ILessonService _lessonService;
    private readonly ITopicService _topicService;


    public QuestionService(IQuestionRepository repository, IOptionRepository optionRepository,
        ISubjectService subjectService, ILessonService lessonService, ITopicService topicService) : base(repository)
    {
        _repository = repository;
        _optionRepository = optionRepository;
        _subjectService = subjectService;
        _lessonService = lessonService;
        _topicService = topicService;
    }

    public async Task<PagedList<Question>> LoadAsync(QuestionParams @params)
    {
        int maxOptionSize = @params.PageSize * 10;

        var query = _repository.Query(@params.WithDeleted);


        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }

        if (@params.SubjectId > 0)
            query = query.Where(x => x.SubjectId == @params.SubjectId);
        if (@params.LessonId > 0)
            query = query.Where(x => x.LessonId == @params.LessonId);
        if (@params.TopicId > 0)
            query = query.Where(x => x.TopicId == @params.TopicId);
        if (@params.ExamId > 0)
        {
            query = query.Where(x => x.ExamId == @params.ExamId);
        }

        if (@params.OrderBy != null)
        {
            query = @params.OrderBy.ToLower() switch
            {
                "name" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name),
                "createdat" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),
                "updatedat" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.UpdatedAt)
                    : query.OrderBy(x => x.UpdatedAt),
                _ => query.OrderByDescending(x => x.Id)
            };
        }
        else
        {
            query = @params.SortBy == "desc"
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);
        }

        var result = await _repository.ExecuteListAsync(query, @params.PageNumber, @params.PageSize);

        var questionIds = result.Items.Select(x => x.Id).ToList();
        var optionQuery = _optionRepository.Query(true)
            .Where(x => questionIds.Contains(x.QuestionId));

        var optionList = (await _optionRepository.ExecuteListAsync(optionQuery, 1, maxOptionSize)).Items;
        var optionGroups = optionList.GroupBy(x => x.QuestionId).ToDictionary(g => g.Key, g => g.ToList());

        var questionList = result.Items.ToList();

        foreach (var question in questionList)
        {
            question.Options = optionGroups.TryGetValue(question.Id, out var options) ? options : [];
            // question.Explanation = ""; // todo: load explanation (only if premium user)
        }

        // todo: get answers (only if premium user)


        return new PagedList<Question>(questionList, result.Count, @params.PageNumber, @params.PageSize);
    }

    public override async Task<Question> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false)
    {
        var query = _repository.Query(withDeleted);

        query = query.Where(x => x.Id == id);

        var entity = await _repository.ExecuteAsync(query);
        if (entity == null) throw new AppException(404, "Topic content not found");

        var options = await _optionRepository.LoadOptionsByQuestionIdAsync(id);
        entity.Options = options;

        return entity;
    }

    public async Task<ApiResponse> SaveAsync(QuestionDto dto)
    {
        var transaction = await BeginTransactionAsync();
        // todo: check options count for a questions, if many more, stop saving

        try
        {
            if (dto.SubjectId > 0)
            {
                var subject = await _subjectService.GetByIdAsync(dto.SubjectId.Value);
                if (subject is null)
                {
                    throw new AppException(400, "Invalid subject");
                }
            }

            if (dto.LessonId > 0)
            {
                var lesson = await _lessonService.GetByIdAsync(dto.LessonId.Value);
                if (lesson is null)
                {
                    throw new AppException(400, "Invalid lesson");
                }
            }

            if (dto.TopicId > 0)
            {
                var topic = await _topicService.GetByIdAsync(dto.TopicId.Value);
                if (topic is null)
                {
                    throw new AppException(400, "Invalid topic");
                }

                if (topic.SubjectId != dto.SubjectId && topic.LessonId != dto.LessonId)
                {
                    // 
                }
            }


            bool duplicateTitle;

            if (dto.Id > 0)
            {
                //todo - fix for update questions
                var existingEntity = await _repository.GetByIdAsync(dto.Id);

                if (existingEntity is null)
                    throw new AppException(404, "Question not found");

                if (existingEntity.Name != dto.Name && existingEntity.Id != dto.Id)
                {
                    duplicateTitle = await _repository.ExistsAsync(x => x.Name == dto.Name);

                    if (duplicateTitle)
                        throw new AppException(400, "A Question with this title already exists");
                }

                // var invalidOptions = dto.Options.Where(x => x.QuestionId != dto.Id).ToList();

                if (dto.Options.Any(x => x.QuestionId != dto.Id && (x.Id > 0)))
                {
                    throw new AppException(400, $"Invalid options found");
                }


                // update options
                // var optionsQuery = _optionRepository.Query(true, true).Where(x => x.QuestionId == dto.Id);
                var optionEntities = await _optionRepository.LoadOptionsByQuestionIdAsync(dto.Id, asTracking: true);

                if (optionEntities.Count > 10)
                {
                    throw new AppException(409, "Too many options, Delete this question");
                }

                var optionsToUpdate = new List<Option>();
                var optionsToDelete = new List<Option>();
                var optionsToCreate = new List<Option>();
                foreach (var option in optionEntities)
                {
                    var optionDto = dto.Options.FirstOrDefault(x => x.Id == option.Id);
                    if (optionDto == null)
                    {
                        optionsToDelete.Add(option);
                        continue;
                    }

                    if (option.Name != optionDto.Name || option.IsCorrect != optionDto.IsCorrect)
                    {
                        option.Name = optionDto.Name;
                        option.IsCorrect = optionDto.IsCorrect;
                        option.UpdatedAt = DateTime.UtcNow;
                        optionsToUpdate.Add(option);
                    }
                }

                foreach (var dtoOption in dto.Options)
                {
                    var exists = optionEntities.Any(x => x.Id == dtoOption.Id);
                    if (!exists)
                    {
                        optionsToCreate.Add(new Option
                        {
                            QuestionId = dto.Id,
                            Name = dtoOption.Name,
                            IsCorrect = dtoOption.IsCorrect,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                if (optionsToUpdate.Any())
                    await _optionRepository.UpdateRangeAsync(optionsToUpdate);
                if (optionsToDelete.Any())
                    await _optionRepository.DeleteRangeAsync(optionsToDelete);
                if (optionsToCreate.Any())
                    await _optionRepository.AddRangeAsync(optionsToCreate);


                MapDtoToEntity(dto, existingEntity);

                await _repository.UpdateAsync(existingEntity);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse(200, "Question updated successfully");
            }

            duplicateTitle = await _repository.ExistsAsync(x => x.Name == dto.Name);

            if (duplicateTitle)
                throw new AppException(400, "A Question with this title already exists");

            var newEntity = MapDtoToEntity(dto);

            await _repository.AddAsync(newEntity);
            await _repository.SaveChangesAsync();

            var options = MapOptionDtoToEntityList(dto.Options, newEntity.Id);

            await _optionRepository.AddRangeAsync(options);

            await _repository.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ApiResponse(200, "Question added successfully");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public Task<ApiResponse> UpdateOptionAsync(long questionId, long optionId, OptionDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<int> RemoveOptionImage(long questionId, long optionId)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedList<QuestionDto>> LoadByTopicIdsAsync(List<long> ids)
    {
        const int maxPageSize = 100;
        const int maxOptionSize = maxPageSize * 10;

        var query = _repository.Query();

        query = query.OrderBy(x => EF.Functions.Random());
        query = query.Where(x => ids.Contains(x.TopicId));

        var result = await _repository.ExecuteListAsync(query, 1, maxPageSize);

        var questionIds = result.Items.Select(x => x.Id).ToList();
        var optionQuery = _optionRepository.Query(true)
            .Where(x => questionIds.Contains(x.QuestionId));

        var optionList = (await _optionRepository.ExecuteListAsync(optionQuery, 1, maxOptionSize)).Items.Select(opt =>
            new OptionDto()
            {
                Id = opt.Id,
                Name = opt.Name,
                IsCorrect = opt.IsCorrect,
                QuestionId = opt.QuestionId,
            });
        var optionGroups = optionList.GroupBy(x => x.QuestionId).ToDictionary(g => g.Key, g => g.ToList());


        var questionList = result.Items.Select(q => new QuestionDto
        {
            Id = q.Id,
            Name = q.Name,
            Marks = q.Marks,
            Options = optionGroups.TryGetValue(q.Id, out var options) ? options : [],
        });


        return new PagedList<QuestionDto>(questionList, result.Count, 1, maxPageSize);
    }

    // public async Task<PagedList<Question>> LoadQuestionsByExamIdAsync(long examId, bool withOptions = false)
    // {
    //     var query = _repository.Query().Where(x => x.ExamId == examId);
    //
    //     var result = await _repository.ExecuteListAsync(query, 1, 200); // max 200 for bcs
    //     
    //     var questionIds = result.Items.Select(x => x.Id).ToList();
    //     var optionQuery = _optionRepository.Query(true)
    //         .Where(x => questionIds.Contains(x.QuestionId));
    // }

    #region Private Methods

    private static Question MapDtoToEntity(QuestionDto dto, Question entity = null)
    {
        entity ??= new Question();

        entity.Id = dto.Id;
        entity.Name = dto.Name;
        entity.UpdatedAt = DateTime.UtcNow;

        entity.SubjectId = dto.SubjectId ?? 0;
        entity.LessonId = dto.LessonId ?? 0;
        entity.TopicId = dto.TopicId ?? 0;

        entity.ExamId = dto.ExamId ?? 0;

        entity.QuestionType = dto.QuestionType;
        entity.Passage = dto.Passage;
        entity.DifficultyLevel = (DifficultyLevel)dto.DifficultyLevel;
        entity.Marks = dto.Marks;
        // entity.Tags = dto.Tags;
        entity.Hint = dto.Hint;
        entity.Explanation = dto.Explanation;

        if (entity.Id > 0)
        {
            entity.UpdatedBy = UserContext.UserId;
        }
        else
        {
            entity.UpdatedBy = UserContext.UserId;
            entity.CreatedBy = UserContext.UserId;
        }

        return entity;
    }

    private static QuestionDto MapEntityToDto(Question subjectEntity)
    {
        return new QuestionDto
        {
            Id = subjectEntity.Id,
            Name = subjectEntity.Name,
        };
    }

    private static List<Option> MapOptionDtoToEntityList(ICollection<OptionDto> dtoOptions, long questionId)
    {
        return dtoOptions.Select(option => new Option
        {
            Id = option.Id,
            Name = option.Name,
            QuestionId = questionId,
            IsCorrect = option.IsCorrect
        }).ToList();
    }

    #endregion
}