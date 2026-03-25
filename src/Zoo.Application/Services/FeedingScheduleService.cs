using Zoo.Domain.Entities;
using Zoo.Domain.Repositories;

namespace Zoo.Application.Services;

public interface IFeedingScheduleService
{
    Task<IEnumerable<FeedingSchedule>> GetTodayFeedingSchedulesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FeedingSchedule>> GetUpcomingFeedingsAsync(TimeSpan within, CancellationToken cancellationToken = default);
    Task MarkFeedingAsCompletedAsync(Guid scheduleId, CancellationToken cancellationToken = default);
}

public class FeedingScheduleService : IFeedingScheduleService
{
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;
    private readonly IAnimalRepository _animalRepository;

    public FeedingScheduleService(
        IFeedingScheduleRepository feedingScheduleRepository,
        IAnimalRepository animalRepository)
    {
        _feedingScheduleRepository = feedingScheduleRepository;
        _animalRepository = animalRepository;
    }

    public async Task<IEnumerable<FeedingSchedule>> GetTodayFeedingSchedulesAsync(CancellationToken cancellationToken = default)
    {
        var allSchedules = await _feedingScheduleRepository.GetActiveSchedulesAsync(cancellationToken);
        var today = DateTime.Today.DayOfWeek;
        return allSchedules.Where(s => s.Days.Contains(today));
    }

    public async Task<IEnumerable<FeedingSchedule>> GetUpcomingFeedingsAsync(TimeSpan within, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var targetTime = now.Add(within);
        
        var allSchedules = await _feedingScheduleRepository.GetActiveSchedulesAsync(cancellationToken);
        return allSchedules.Where(s => 
        {
            var feedingDateTime = DateTime.Today.AddHours(s.FeedingTime.Hour).AddMinutes(s.FeedingTime.Minute);
            return feedingDateTime > now && feedingDateTime <= targetTime;
        });
    }

    public Task MarkFeedingAsCompletedAsync(Guid scheduleId, CancellationToken cancellationToken = default)
    {
        // Implementation for marking feeding as completed
        // This could log the feeding event, notify keepers, etc.
        return Task.CompletedTask;
    }
}
