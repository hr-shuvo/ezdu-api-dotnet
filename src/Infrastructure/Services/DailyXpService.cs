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

    public async Task<int> AddXpAsync(int newXp)
    {
        var userId = UserContext.UserId;
        var today = DateTime.UtcNow.Date;

        var (totalXpAfterLastFriday, todayXp) = await ProcessDailyXpAsync(userId, today);

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

        return totalXpAfterLastFriday + newXp;
    }

    private async Task<(int totalXpAfterLastFriday, DailyXp todayXp)> ProcessDailyXpAsync(long userId,  DateTime today)
    {
        var lastFriday = GetLastFriday(today);
        
        var userDailyXps = await _repository.Query()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Day)
            .ToListAsync();
        
        var todayXp = userDailyXps.FirstOrDefault(x => x.Day.Date == today);
        
        var totalXpAfterLastFriday = userDailyXps
            .Where(x => x.Day.Date > lastFriday)
            .Sum(x => x.Xp);
        

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
        
        return (totalXpAfterLastFriday, todayXp);
    }
    
    private static DateTime GetLastFriday(DateTime today)
    {
        var daysSinceFriday = ((int)today.DayOfWeek - (int)DayOfWeek.Friday + 7) % 7;
        return today.AddDays(-daysSinceFriday);
    }
}