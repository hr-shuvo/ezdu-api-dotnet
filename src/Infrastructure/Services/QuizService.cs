using Core.App.Repositories.Interfaces;
using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class QuizService : BaseService<Quiz>, IQuizService
{
    public QuizService(IQuizRepository repository) : base(repository)
    {
    }
}