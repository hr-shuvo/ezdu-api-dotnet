using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class QuestionService : BaseService<Question>, IQuestionService
{
    public QuestionService(IQuestionRepository repository) : base(repository)
    {
    }
}