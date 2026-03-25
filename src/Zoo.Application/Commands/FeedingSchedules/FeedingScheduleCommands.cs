using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;

namespace Zoo.Application.Commands.FeedingSchedules;

public record CreateFeedingScheduleCommand(
    Guid AnimalId,
    int FeedingHour,
    int FeedingMinute,
    FoodType FoodType,
    decimal QuantityAmount,
    string QuantityUnit,
    List<DayOfWeek> Days,
    string? SpecialInstructions = null
) : ICommand<Guid>;

public class CreateFeedingScheduleCommandHandler : ICommandHandler<CreateFeedingScheduleCommand, Guid>
{
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;
    private readonly IAnimalRepository _animalRepository;

    public CreateFeedingScheduleCommandHandler(
        IFeedingScheduleRepository feedingScheduleRepository,
        IAnimalRepository animalRepository)
    {
        _feedingScheduleRepository = feedingScheduleRepository;
        _animalRepository = animalRepository;
    }

    public async Task<Guid> HandleAsync(CreateFeedingScheduleCommand command, CancellationToken cancellationToken = default)
    {
        // Verify animal exists
        _ = await _animalRepository.GetByIdAsync(command.AnimalId, cancellationToken)
            ?? throw new AnimalNotFoundException(command.AnimalId);

        var feedingTime = new FeedingTime(command.FeedingHour, command.FeedingMinute);
        var quantity = new Quantity(command.QuantityAmount, command.QuantityUnit);

        var schedule = FeedingSchedule.Create(
            command.AnimalId,
            feedingTime,
            command.FoodType,
            quantity,
            command.Days,
            command.SpecialInstructions
        );

        await _feedingScheduleRepository.AddAsync(schedule, cancellationToken);
        
        return schedule.Id;
    }
}
