using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class AchievementService: BaseService<Achievement>, IAchievementService
{
    public AchievementService(IAchievementRepository repository) : base(repository)
    {
    }
}