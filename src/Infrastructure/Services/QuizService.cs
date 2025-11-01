using Core.App.Services;
using Core.App.Services.Interfaces;
using Core.App.Utils;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.Extensions;
using Core.QueryParams;
using Core.Repositories;
using Core.Services;
using Core.Shared.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuizService : BaseService<Quiz>, IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;

    public QuizService(IQuizRepository repository, IUnitOfWork unitOfWork) : base(repository)
    {
        _quizRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<Quiz>> LoadAsync(QuizParams @params)
    {
        var query = _quizRepository.Query(@params.WithDeleted);

        // TODO: Add more filters as needed
        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
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
                "starttime" => query.OrderBy(x => x.StartTime < DateTime.UtcNow)
                    .ThenByDescending(x => x.StartTime),
                _ => query.OrderByDescending(x => x.Id)
            };
        }
        else
        {
            query = @params.SortBy == "desc"
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);
        }

        var result = await _quizRepository.ExecuteListAsync(query, @params.PageNumber, @params.PageSize);

        return new PagedList<Quiz>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public override async Task<Quiz> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false)
    {
        var result = await _quizRepository.GetByIdAsync(id, asTracking, withDeleted);

        if (result is not null)
        {
            // var quizQuestions = await _unitOfWork.QuizQuestions.GetByQuizIdAsync(id);
            // result.Questions = quizQuestions;
        }

        return result;
    }

    public async Task<QuizResponseDto> GetByIdWithQuestionsAsync(long id, bool asTracking = false,
        bool withDeleted = false)
    {
        var result = await _quizRepository.GetByIdAsync(id, asTracking, withDeleted);

        if (result == null)
            throw new AppException(404, "Quiz not found");

        var quizQuestions = await _unitOfWork.QuizQuestions.GetByQuizIdAsync(id);
        var questionIds = quizQuestions.Select(x => x.QuestionId).ToList();

        var questions = await _unitOfWork.Repository<Question>().Query().Where(x => questionIds.Contains(x.Id))
            .ToListAsync();

        var options = await _unitOfWork.Repository<Option>().Query().Where(x => questionIds.Contains(x.QuestionId))
            .ToListAsync();

        var quizDto = MapEntityToResponseDto(result, questions, options);

        return quizDto;
    }


    public async Task<ApiResponse> SaveAsync(QuizDto dto)
    {
        var transaction = await BeginTransactionAsync();

        try
        {
            bool duplicateName;


            if (dto.Id > 0)
            {
                var existingEntity = await _quizRepository.GetByIdAsync(dto.Id);

                if (existingEntity is null)
                    throw new AppException(404, "Quiz not found");

                if (existingEntity.Name != dto.Name)
                {
                    duplicateName = await _quizRepository.ExistsAsync(x => x.Name == dto.Name);

                    if (duplicateName)
                        throw new AppException(400, "A Quiz with this Name already exists");
                }

                MapDtoToEntity(dto, existingEntity);

                await _quizRepository.UpdateAsync(existingEntity);
                await _quizRepository.SaveChangesAsync();

                await HandleQuizQuestions(dto.Id, dto.QuestionIds);

                await transaction.CommitAsync();
                return new ApiResponse(200, "Quiz updated successfully");
            }

            duplicateName = await _quizRepository.ExistsAsync(x => x.Name == dto.Name);

            if (duplicateName)
                throw new AppException(400, "A Quiz with this Name already exists");

            var newEntity = MapDtoToEntity(dto);

            await _quizRepository.AddAsync(newEntity);
            await _quizRepository.SaveChangesAsync();

            await HandleQuizQuestions(newEntity.Id, dto.QuestionIds);

            await transaction.CommitAsync();
            return new ApiResponse(200, "Quiz added successfully");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    #region Private Methods

    private static Quiz MapDtoToEntity(QuizDto dto, Quiz entity = null)
    {
        entity ??= new Quiz { CreatedBy = UserContext.UserId };

        entity.Id = dto.Id;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Type = dto.Type;
        entity.TotalMarks = dto.TotalMarks;
        entity.PassingMarks = dto.PassingMarks;
        entity.DurationInMinutes = dto.DurationInMinutes;
        entity.HasNegativeMarks = dto.HasNegativeMarks;
        entity.StartTime = dto.StartTime.ToUtcSafe();
        entity.EndTime = dto.EndTime.ToUtcSafe() ?? dto.StartTime.ToUtcSafe()?.AddMinutes(dto.DurationInMinutes);

        return entity;
    }

    private static QuizResponseDto MapEntityToResponseDto(Quiz result, List<Question> questions, List<Option> options)
    {
        if (result == null) return null;

        var mappedQuestions = questions
            .Select(q =>
            {
                q.Options = options
                    .Where(o => o.QuestionId == q.Id)
                    .ToList();

                return q;
            })
            .ToList();

        var dto = new QuizResponseDto
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Type = result.Type,
            TotalMarks = result.TotalMarks,
            PassingMarks = result.PassingMarks,
            DurationInMinutes = result.DurationInMinutes,
            HasNegativeMarks = result.HasNegativeMarks,
            StartTime = result.StartTime,
            EndTime = result.EndTime,
            Status = result.Status,

            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
            CreatedBy = result.CreatedBy,
            UpdatedBy = result.UpdatedBy,

            Questions = mappedQuestions,
        };

        return dto;
    }

    private async Task HandleQuizQuestions(long quizId, ICollection<long> dtoQuestionIds)
    {
        var existing = await _unitOfWork.QuizQuestions.GetByQuizIdAsync(quizId);
        var existingIds = existing.Select(x => x.QuestionId).ToList();

        var toAdd = dtoQuestionIds.Except(existingIds).ToList();
        var toRemove = existingIds.Except(dtoQuestionIds).ToList();

        if (toAdd.Count > 0)
        {
            var newPairs = toAdd.Select(qId => new QuizQuestion
            {
                QuizId = quizId,
                QuestionId = qId
            });

            await _unitOfWork.QuizQuestions.AddQuizQuestionsAsync(newPairs);
        }

        if (toRemove.Count > 0)
        {
            await _unitOfWork.QuizQuestions.RemoveQuizQuestionsAsync(quizId, toRemove);
        }

        await _unitOfWork.CompleteAsync();
    }

    #endregion
}