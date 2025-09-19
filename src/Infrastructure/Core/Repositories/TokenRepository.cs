using Core.App.Entities.Identity;
using Core.App.Repositories.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Repositories;

public class TokenRepository : BaseRepository<AuthToken>, ITokenRepository
{
    public TokenRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> DeleteAllByUserIdAsync(long userId)
    {
        var query = DbSet.AsQueryable();
        
        var tokensToDelete = query.Where(x => x.UserId == userId);

        var result = await query.ExecuteDeleteAsync() > 0;
        
        return result;
    }
}