using Core.App.Models;
using Core.Entities;
using Core.QueryParams;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClassRepository : BaseRepository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(int Count, List<Class> Items)> LoadAsync(ClassParams @params)
    {
        var query = DbSet.AsQueryable();

        if (@params.WithDeleted == false)
        {
            query = query.Where(x => x.Status != Status.Deleted);
        }

        //TODO: Apply additional filters based on ClassParams properties

        // Pagination
        var count = await query.CountAsync();
        var items = await query
            .Skip((@params.PageNumber - 1) * @params.PageSize)
            .Take(@params.PageSize)
            .ToListAsync();

        return (count, items);
    }
}