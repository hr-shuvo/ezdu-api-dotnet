using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class TopicRepository:BaseRepository<Topic>, ITopicRepository
{
    public TopicRepository(AppDbContext context) : base(context)
    {
    }
}