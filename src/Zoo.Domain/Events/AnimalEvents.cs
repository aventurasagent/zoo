using Zoo.Domain.Entities;

namespace Zoo.Domain.Events;

public class AnimalMovedToEnclosure : DomainEvent
{
    public Guid AnimalId { get; }
    public Guid EnclosureId { get; }

    public AnimalMovedToEnclosure(Guid animalId, Guid enclosureId)
    {
        AnimalId = animalId;
        EnclosureId = enclosureId;
    }
}

public class AnimalStatusChanged : DomainEvent
{
    public Guid AnimalId { get; }
    public AnimalStatus NewStatus { get; }

    public AnimalStatusChanged(Guid animalId, AnimalStatus newStatus)
    {
        AnimalId = animalId;
        NewStatus = newStatus;
    }
}

public class AnimalAddedToEnclosure : DomainEvent
{
    public Guid EnclosureId { get; }
    public Guid AnimalId { get; }

    public AnimalAddedToEnclosure(Guid enclosureId, Guid animalId)
    {
        EnclosureId = enclosureId;
        AnimalId = animalId;
    }
}

public class AnimalRemovedFromEnclosure : DomainEvent
{
    public Guid EnclosureId { get; }
    public Guid AnimalId { get; }

    public AnimalRemovedFromEnclosure(Guid enclosureId, Guid animalId)
    {
        EnclosureId = enclosureId;
        AnimalId = animalId;
    }
}

public class FeedingTimeApproaching : DomainEvent
{
    public Guid FeedingScheduleId { get; }
    public Guid AnimalId { get; }
    public TimeSpan TimeUntilFeeding { get; }

    public FeedingTimeApproaching(Guid feedingScheduleId, Guid animalId, TimeSpan timeUntilFeeding)
    {
        FeedingScheduleId = feedingScheduleId;
        AnimalId = animalId;
        TimeUntilFeeding = timeUntilFeeding;
    }
}

public class AnimalFed : DomainEvent
{
    public Guid AnimalId { get; }
    public DateTime FedAt { get; }
    public string FoodType { get; }

    public AnimalFed(Guid animalId, DateTime fedAt, string foodType)
    {
        AnimalId = animalId;
        FedAt = fedAt;
        FoodType = foodType;
    }
}
