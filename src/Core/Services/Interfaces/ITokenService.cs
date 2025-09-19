using Core.App.Entities.Identity;

namespace Core.Services.Interfaces;

public interface ITokenService : IBaseService<AuthToken>
{
    string CreateAuthToken(AppUser user);
    
    string EncryptCode(int code);
    int DecryptCode(string encryptedCode);
}