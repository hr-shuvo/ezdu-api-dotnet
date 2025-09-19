using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class AchievementRepository:BaseRepository<Achievement>, IAchievementRepository
{
    public AchievementRepository(AppDbContext context) : base(context)
    {
    }
}