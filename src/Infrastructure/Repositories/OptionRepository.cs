using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OptionRepository : BaseRepository<Option>, IOptionRepository
{
    public OptionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Option>> LoadOptionsByQuestionIdAsync(long questionId, bool asTracking = false)
    {
        var query = Query(true, asTracking);

        return await query.Where(x => x.QuestionId == questionId).ToListAsync();
    }

    public async Task<List<Option>> AddRangeAsync(List<Option> options)
    {
        if (options == null || options.Count == 0)
            return options;

        foreach (var option in options)
        {
            option.CreatedAt = DateTime.UtcNow;
            option.UpdatedAt = DateTime.UtcNow;
        }
        
        await DbSet.AddRangeAsync(options);

        return options;
    }


    public async Task<List<Option>> UpdateRangeAsync(List<Option> options)
    {
        if (options == null || options.Count == 0)
            return options;

        foreach (var option in options)
        {
            option.UpdatedAt = DateTime.UtcNow;
        }

        DbSet.UpdateRange(options);

        await SaveChangesAsync();

        return options;
    }

    public async Task DeleteRangeAsync(List<Option> optionsToDelete)
    {
        DbSet.RemoveRange(optionsToDelete);
        
        await SaveChangesAsync();
    }
}