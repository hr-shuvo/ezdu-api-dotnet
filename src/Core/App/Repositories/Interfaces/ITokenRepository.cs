using Core.App.Entities.Identity;

namespace Core.App.Repositories.Interfaces;

public interface ITokenRepository : IBaseRepository<AuthToken>
{
    Task<bool> DeleteAllByUserIdAsync(long userId);
}