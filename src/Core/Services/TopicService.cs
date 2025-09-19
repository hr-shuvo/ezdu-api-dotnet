using Core.App.Services;
using Core.Entities;
using Core.Repositories;

namespace Core.Services;

public class TopicService : BaseService<Topic>, ITopicService
{
    public TopicService(ITopicRepository repository) : base(repository)
    {
    }
}