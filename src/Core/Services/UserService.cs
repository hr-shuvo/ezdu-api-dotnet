using Core.App.DTOs;
using Core.App.Entities.Identity;
using Core.App.Models;
using Core.QueryParams;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public class UserService : BaseService<AppUser>, IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository repository) : base(repository)
    {
        _userRepository = repository;
    }

    public async Task<PagedList<UserDto>> GetUsersAsync(UserParams query)
    {
        var result = await _userRepository.LoadAsync(query);
        
        var users = result.Items.Select(u => {
            // var mainPhoto = u.Photos.FirstOrDefault(p => p.IsMain)?.Url;
            return new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Name = u.UserName,
                CreatedAt = u.CreatedAt,
                
                Status = u.Status == Status.Active ? "Active" : "Inactive",
                
                // LastActive = u.LastActive,
                
                // Age = u.DateOfBirth.CalculateAge(),
                // PhotoUrl = mainPhoto,
            };
            
        }).ToList();
        
        return new PagedList<UserDto>(users, result.Count, query.PageNumber, query.PageSize);
    }
}