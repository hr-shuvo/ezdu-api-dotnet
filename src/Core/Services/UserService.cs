using Core.App.Entities.Identity;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Core.Services;

public class UserService : BaseService<AppUser>, IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository repository) : base(repository)
    {
        _userRepository = repository;
    }

}