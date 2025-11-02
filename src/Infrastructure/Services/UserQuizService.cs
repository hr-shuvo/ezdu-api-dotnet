using Core.App.Repositories.Interfaces;
using Core.App.Services;
using Core.App.Utils;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Errors;
using Core.Extensions;
using Core.QueryParams;
using Core.Services;
using Core.Shared.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserQuizService : BaseService<UserQuiz>, IUserQuizService
{
    private const int MaxQuizSize = 20;

    private readonly IBaseRepository<UserQuiz> _repository;
    private readonly IQuizService _quizService;
    private readonly IExamArchiveService _examArchiveService;
    private readonly IProgressService _progressService;

    public UserQuizService(IBaseRepository<UserQuiz> repository, IQuizService quizService,
        IExamArchiveService examArchiveService, IProgressService progressService) :
        base(repository)
    {
        _repository = repository;
        _quizService = quizService;
        _examArchiveService = examArchiveService;
        _progressService = progressService;
    }

    public async Task<PagedList<UserQuiz>> LoadAsync(UserQuizParams @params)
    {
        var userId = @params.UserId > 0 ? @params.UserId : UserContext.UserId;

        var query = _repository.Query()
            .Where(x => x.UserId == userId);

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

        return new PagedList<UserQuiz>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<Progress> SaveAsync(UserQuizDto dto)
    {
        var userId = UserContext.UserId;

        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            await CleanupOldQuizzesAsync(userId);

            var newEntity = MapDtoToEntity(dto);
            newEntity.Name = await GetQuizNameAsync(dto);


            // add to the database
            await _repository.AddAsync(newEntity);
            // update progress to add xp
            var progress = await _progressService.AddXpAsync(newEntity.Xp);
            // save database
            await _repository.SaveChangesAsync();

            await transaction.CommitAsync();

            return progress;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    #region Private Methods

    private UserQuiz MapDtoToEntity(UserQuizDto dto)
    {
        var entity = new UserQuiz(UserContext.UserId)
        {
            QuizType = dto.QuizType,
            QuizId = dto.QuizType is QuizType.Quiz or QuizType.Archive ? dto.QuizId : 0,
            MarkPercentage = dto.MarkPercentage,
            Xp = dto.MarkPercentage.XpByPercentage()
        };

        return entity;
    }

    private async Task CleanupOldQuizzesAsync(long userId)
    {
        var userQuizzes = await _repository.Query()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Time)
            .ToListAsync();

        if (userQuizzes.Count >= MaxQuizSize)
        {
            var removeCount = userQuizzes.Count - MaxQuizSize + 1;
            var quizToRemove = userQuizzes.Take(removeCount).ToList();

            if (quizToRemove.Any())
            {
                await _repository.RemoveRangeAsync(quizToRemove);
                // await _repository.SaveChangesAsync(); // call it to end
            }
        }
    }

    private async Task<string> GetQuizNameAsync(UserQuizDto dto)
    {
        switch (dto.QuizType)
        {
            case QuizType.Quiz:
            {
                var quiz = await _quizService.GetByIdAsync(dto.QuizId);
                return quiz is null ? "Unavailable (updated or removed)" : quiz.Name;
            }
            case QuizType.Archive:
            {
                var quiz = await _examArchiveService.GetByIdAsync(dto.QuizId);
                return quiz is null ? "Unavailable (updated or removed)" : quiz.Name;
            }
            case QuizType.Mock:
            default:
                return "Personalized Mock";
        }
    }

    #endregion
}