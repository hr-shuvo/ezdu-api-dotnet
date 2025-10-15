using Core.Entities;
using Core.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuizQuestionRepository : IQuizQuestionRepository
{
    private readonly DbSet<QuizQuestion> _quizQuestions;

    public QuizQuestionRepository(AppDbContext context)
    {
        _quizQuestions = context.Set<QuizQuestion>();
    }

    public async Task<List<QuizQuestion>> GetByQuizIdAsync(long quizId)
    {
        return await _quizQuestions
            .Where(x => x.QuizId == quizId)
            .ToListAsync();
    }

    public async Task AddQuizQuestionsAsync(IEnumerable<QuizQuestion> newPairs)
    {
        await _quizQuestions.AddRangeAsync(newPairs);
    }

    public async Task RemoveQuizQuestionsAsync(long quizId, List<long> questionIds)
    {
        var entitiesToRemove = await _quizQuestions
            .Where(x => x.QuizId == quizId && questionIds.Contains(x.QuestionId))
            .ToListAsync();

        if (entitiesToRemove.Count > 0)
            _quizQuestions.RemoveRange(entitiesToRemove);
    }
}