using Core.App.Entities.Identity;
using Core.Repositories.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserRepository:  IUserRepository
{
    public UserRepository(AppDbContext context)
    {
    }
}