using Core.App.Repositories.Interfaces;
using Core.App.Services;
using Core.App.Utils;
using Core.Entities;
using Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class DailyXpService : BaseService<DailyXp>, IDailyXpService
{
    private readonly IBaseRepository<DailyXp> _repository;
    private const int MaxDailyXp = 50;

    public DailyXpService(IBaseRepository<DailyXp> repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task AddXpAsync(int newXp)
    {
        var userId = UserContext.UserId;
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        await CleanupOldDailyXpAsync(userId);

        var todayXp = await _repository.GetAsync(x => x.UserId == userId && x.Day >= today && x.Day < tomorrow);
        if (todayXp is null)
        {
            await _repository.AddAsync(new DailyXp { UserId = userId, Day = today, Xp = newXp });
        }
        else
        {
            todayXp.Xp += newXp;
            await _repository.UpdateAsync(todayXp);
        }

        // await _repository.SaveChangesAsync();
    }

    private async Task CleanupOldDailyXpAsync(long userId)
    {
        var userDailyXps = await _repository.Query()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Day)
            .ToListAsync();

        if (userDailyXps.Count > MaxDailyXp)
        {
            var removeCount = userDailyXps.Count - MaxDailyXp;
            var oldDailyXpsToRemove = userDailyXps.Take(removeCount).ToList();

            if (oldDailyXpsToRemove.Count != 0)
            {
                await _repository.RemoveRangeAsync(oldDailyXpsToRemove);
                // NOTE: Do NOT call SaveChangesAsync() here.
            }
        }
    }
}