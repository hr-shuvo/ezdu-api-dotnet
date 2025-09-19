using System.Linq.Expressions;
using Core.App.Entities.Identity;
using Core.App.Models;
using Core.App.Repositories.Interfaces;
using Core.QueryParams;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<AppUser>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(int Count, List<AppUser> Items)> LoadAsync(UserParams @params)
    {
        var query = DbSet.AsQueryable();

        if (@params.IsDeleted != true)
        {
            query = query.Where(x => x.Status != Status.Deleted);
        }
        
        // Apply additional filters based on UserParams
        // TODO: Implement filtering logic based on UserParams properties
        
        // Pagination
        var count = await query.CountAsync();
        var items = await query
            .Skip((@params.PageNumber - 1) * @params.PageSize)
            .Take(@params.PageSize)
            .ToListAsync();
        
        return (count, items);
    }
}