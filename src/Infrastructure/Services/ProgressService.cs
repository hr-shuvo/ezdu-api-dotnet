using System.Linq.Expressions;
using Core.App.Services;
using Core.App.Services.Interfaces;
using Core.App.Utils;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class ProgressService : BaseService<Progress>, IProgressService
{
    private readonly IProgressRepository _repository;

    // private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IDailyXpService _dailyXpService;

    public ProgressService(IProgressRepository repository, IUserService userService, IDailyXpService dailyXpService) :
        base(repository)
    {
        _repository = repository;
        _userService = userService;
        _dailyXpService = dailyXpService;
    }

    public override async Task<Progress> GetAsync(Expression<Func<Progress, bool>> predicate, bool asTracking = false,
        bool withDeleted = false)
    {
        var result = await _repository.GetAsync(x => x.UserId == UserContext.UserId);

        if (result is not null) return result;

        return await CreateIfNotExists();
    }

    public async Task<IEnumerable<Progress>> LoadProgress(long userId)
    {
        var result =
            await _repository.LoadAsync(page: 1, size: 2, x => x.UserId == UserContext.UserId || x.UserId == userId);

        return result.Items;
    }

    public async Task AddXpAsync(int newXp)
    {
        if (newXp < 1) return;

        var userId = UserContext.UserId;
        
        var progress = await _repository.GetAsync(x => x.UserId == userId) ?? await CreateIfNotExists();
        var today = DateTime.UtcNow.Date;
        var lastStreakDay = progress.LastStreakDay.Date;

        progress.TotalXp += newXp;
        progress.LastStreakDay = today;
        progress.StreakCount = lastStreakDay == today
            ? progress.StreakCount
            : lastStreakDay == today.AddDays(-1)
                ? progress.StreakCount + 1
                : 1;
        // todo: add max streak 


        await _repository.UpdateAsync(progress);
        // await _repository.SaveChangesAsync();

        await _dailyXpService.AddXpAsync(newXp);
    }


    #region Private Methods

    private async Task<Progress> CreateIfNotExists()
    {
        var user = await _userService.GetByIdAsync(UserContext.UserId);
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

        var result = await _repository.AddAsync(progress);
        await _repository.SaveChangesAsync();

        return result;
    }

    #endregion
}