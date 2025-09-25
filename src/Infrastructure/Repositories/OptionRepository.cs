using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class OptionRepository : BaseRepository<Option>, IOptionRepository
{
    
    public OptionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Option>> AddRangeAsync(List<Option> options)
    {
        await DbSet.AddRangeAsync(options);
        
        return options;
    }
}