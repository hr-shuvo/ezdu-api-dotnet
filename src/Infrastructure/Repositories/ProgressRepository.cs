using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProgressRepository : BaseRepository<Progress>, IProgressRepository
{
    public ProgressRepository(AppDbContext context) : base(context)
    {
    }

    public async Task ResetWeeklyProgressAsync()
    {
        const int batchSize = 500;
        var totalAffected = 0;
        
        while (true)
        {
            var rowsAffected = await DbSet
                .Where(x => x.LastResetDate < DateTime.UtcNow.AddDays(-7))
                .Take(batchSize)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.WeekXp, 0)
                    .SetProperty(p => p.LastResetDate, DateTime.UtcNow));
        
            totalAffected += rowsAffected;
            if (rowsAffected < batchSize) break;
        }
    }
}