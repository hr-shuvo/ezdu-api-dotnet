using Core.App.Entities.Identity;
using Core.QueryParams;

namespace Core.App.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<AppUser>
{
    Task<(int Count, List<AppUser> Items)> LoadAsync(UserParams querySearch);
}