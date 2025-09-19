using Core.App.Repositories.Interfaces;
using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class TopicContentService : BaseService<TopicContent>, ITopicContentService
{
    public TopicContentService(ITopicContentRepository repository) : base(repository)
    {
    }
}