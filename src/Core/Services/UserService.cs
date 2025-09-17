using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository repository)
    {
        _userRepository = repository;
    }

}