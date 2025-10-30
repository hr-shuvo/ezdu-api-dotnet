using System.Linq.Expressions;
using Core.App.Repositories.Interfaces;
using Core.App.Services;
using Core.App.Utils;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class ProgressService : BaseService<Progress>, IProgressService
{
    private readonly IProgressRepository _repository;
    private readonly IUserRepository _userRepository;

    public ProgressService(IProgressRepository repository, IUserRepository userRepository) : base(repository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public override async Task<Progress> GetAsync(Expression<Func<Progress, bool>> predicate, bool asTracking = false,
        bool withDeleted = false)
    {
        var result = await _repository.GetAsync(x => x.UserId == UserContext.UserId);

        if (result is not null) return result;

        var user = await _userRepository.GetByIdAsync(UserContext.UserId);
        if (user is null)
        {
            throw new AppException(401, "User does not exist");
        }

        var progress = new Progress(UserContext.UserId)
        {
            UserName = user.UserName,
            Name = user.UserName,
            // UserImageUrl = user.ImageUrl;// todo,
            TotalXp = 0,
            StreakCount = 0,
        };

        result = await _repository.AddAsync(progress);
        await _repository.SaveChangesAsync();

        return result;
    }

    public virtual async Task<IEnumerable<Progress>> LoadProgress(long userId)
    {
        var result =
            await _repository.LoadAsync(page: 1, size: 2, x => x.UserId == UserContext.UserId || x.UserId == userId);

        return result.Items;
    }
}