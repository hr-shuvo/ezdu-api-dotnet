using Core.Entities.Identity;

namespace Core.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}