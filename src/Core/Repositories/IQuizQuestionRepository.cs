using Core.Entities;

namespace Core.Repositories;

public interface IQuizQuestionRepository
{
    Task<List<QuizQuestion>> GetByQuizIdAsync(long quizId);
    Task AddQuizQuestionsAsync(IEnumerable<QuizQuestion> newPairs);
    Task RemoveQuizQuestionsAsync(long quizId, List<long> toRemove);
}