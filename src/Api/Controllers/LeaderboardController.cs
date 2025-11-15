using Core.App.Controllers;
using Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[Authorize]
public class LeaderboardController : BaseApiController
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }
    
    
}