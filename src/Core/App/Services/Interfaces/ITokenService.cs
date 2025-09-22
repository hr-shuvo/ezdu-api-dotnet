using Core.App.Entities.Identity;

namespace Core.App.Services.Interfaces;

public interface ITokenService : IBaseService<AuthToken>
{
    string CreateAuthToken(AppUser user, IList<string> roles);
    
    Task<bool> DeleteAllByUserIdAsync(long userId);
    
    string EncryptCode(int code);
    int DecryptCode(string encryptedCode);
}