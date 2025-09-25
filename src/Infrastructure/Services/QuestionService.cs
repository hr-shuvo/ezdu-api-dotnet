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

        // TODO: Add more filters as needed
        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }


        if (@params.OrderBy != null)
        {
            query = @params.SortBy.ToLower() switch
            {
                "name" => @params.OrderBy == "desc"
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name),
                "createdat" => @params.OrderBy == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),
                "updatedat" => @params.OrderBy == "desc"
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
        }

        return new PagedList<Question>(questionList, result.Count, @params.PageNumber, @params.PageSize);
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
                var existingEntity = await _repository.GetByIdAsync(dto.Id);

                if (existingEntity is null)
                    throw new AppException(404, "Subject not found");

                if (existingEntity.Name != dto.Name)
                {
                    duplicateTitle = await _repository.ExistsAsync(x => x.Name == dto.Name);

                    if (duplicateTitle)
                        throw new AppException(400, "A Subject with this title already exists");
                }

                MapDtoToEntity(dto, existingEntity);

                await _repository.UpdateAsync(existingEntity);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse(200, "Subject updated successfully");
            }

            duplicateTitle = await _repository.ExistsAsync(x => x.Name == dto.Name);

            if (duplicateTitle)
                throw new AppException(400, "A Subject with this title already exists");

            var newEntity = MapDtoToEntity(dto);

            await _repository.AddAsync(newEntity);
            await _repository.SaveChangesAsync();

            var options = MapOptionDtoToEntityList(dto.Options, newEntity.Id);

            var optionEntities = await _optionRepository.AddRangeAsync(options);

            await _repository.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ApiResponse(200, "Subject added successfully");
        }
        catch (Exception ex)
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

    #region Private Methods

    private static Question MapDtoToEntity(QuestionDto dto, Question entity = null)
    {
        entity ??= new Question();

        entity.Id = dto.Id;
        entity.Name = dto.Name;
        entity.UpdatedAt = DateTime.UtcNow;

        entity.SubjectId = dto.SubjectId.Value;
        entity.LessonId = dto.LessonId.Value;
        entity.TopicId = dto.TopicId.Value;

        entity.QuestionType = (QuestionType)dto.QuestionType;
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
            Name = option.Text,
            QuestionId = questionId,
            IsCorrect = option.IsCorrect
        }).ToList();
    }

    #endregion
}