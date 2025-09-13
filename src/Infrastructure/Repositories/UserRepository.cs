using Core.Entities.Identity;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<AppUser>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}