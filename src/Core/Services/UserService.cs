using Core.Entities.Identity;
using Core.Services.Interfaces;
using Domain.Interfaces.Repositories;

namespace Core.Services;

public class UserService : BaseService<AppUser>, IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository repository) : base(repository)
    {
        _userRepository = repository;
    }

}