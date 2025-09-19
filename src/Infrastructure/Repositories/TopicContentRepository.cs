using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class TopicContentRepository: BaseRepository<TopicContent>, ITopicContentRepository
{
    public TopicContentRepository(AppDbContext context) : base(context)
    {
    }
}