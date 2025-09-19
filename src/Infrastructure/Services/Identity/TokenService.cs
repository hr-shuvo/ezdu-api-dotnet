using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.App.Entities.Identity;
using Core.Repositories.Interfaces;
using Core.Services;
using Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Identity;

public class TokenService : BaseService<AuthToken>, ITokenService
{
    private readonly ITokenRepository _repository;
    
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    private readonly string _encKey;
    private readonly string _encSalt;

    public TokenService(IConfiguration config, ITokenRepository repo) : base(repo)
    {
        _repository = repo;
        
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]!));

        _encKey = _config["Token:EncryptionKey"] ?? "0123456789ABCDEF";
        _encSalt = _config["Token:EncryptionSalt"] ?? "unique-salt-value";
    }


    public string CreateAuthToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            // Issuer = _config["Token:Issuer"]
        };


        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    
    
    
    public async Task<bool> DeleteAllByUserIdAsync(long userId)
    {
        return await _repository.DeleteAllByUserIdAsync(userId);
    }





    #region Encryption and Decryption

    
    public string EncryptCode(int code)
    {
        byte[] codeBytes = BitConverter.GetBytes(code);
        
        using var aes = Aes.Create();
        
        aes.Key = Encoding.UTF8.GetBytes(_encKey.PadRight(32).Substring(0, 32));
        aes.IV = Encoding.UTF8.GetBytes(_encSalt.PadRight(16).Substring(0, 16));
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        
        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] cipherBytes = encryptor.TransformFinalBlock(codeBytes, 0, codeBytes.Length);
        
        return Convert.ToBase64String(cipherBytes);
    }

    public int DecryptCode(string encryptedCode)
    {
        byte[] cipherBytes = Convert.FromBase64String(encryptedCode);
        
        using var aes = Aes.Create();
        
        aes.Key = Encoding.UTF8.GetBytes(_encKey.PadRight(32).Substring(0, 32));
        aes.IV = Encoding.UTF8.GetBytes(_encSalt.PadRight(16).Substring(0, 16));
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        byte[] codeBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        
        return BitConverter.ToInt32(codeBytes, 0);
    }

    #endregion

}