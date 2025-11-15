using Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs;

public class WeeklyResetService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WeeklyResetService> _logger;

    public WeeklyResetService(IServiceProvider serviceProvider, ILogger<WeeklyResetService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = GetNextResetMidnight(now);
            var delay = nextRun - now;

            _logger.LogInformation($"Next weekly reset scheduled for {nextRun}");

            await Task.Delay(delay, stoppingToken);


            if (!stoppingToken.IsCancellationRequested)
            {
                await ResetUserProgress();
            }
        }
    }


    #region Private Methods

    private DateTime GetNextResetMidnight(DateTime now)
    {
        const DayOfWeek resetDay = DayOfWeek.Friday;

        var daysUntilMonday = ((int)resetDay - (int)now.DayOfWeek + 7) % 7;
        if (daysUntilMonday == 0 && now.TimeOfDay.TotalSeconds > 0)
            daysUntilMonday = 7;

        return now.Date.AddDays(daysUntilMonday);
    }

    private async Task ResetUserProgress()
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IProgressRepository>();
                await repository.ResetWeeklyProgressAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in weekly reset");
        }
    }

    #endregion
}