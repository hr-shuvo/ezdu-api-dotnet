using Core.App.Services.Interfaces;
using Core.Entities;

namespace Core.Services;

public interface IDailyXpService : IBaseService<DailyXp>
{
    Task AddXpAsync(int newXp);
}