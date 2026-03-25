namespace Zoo.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception inner) : base(message, inner) { }
}

public class AnimalNotFoundException : DomainException
{
    public AnimalNotFoundException(Guid animalId) 
        : base($"Animal with ID {animalId} was not found") { }
}

public class EnclosureNotFoundException : DomainException
{
    public EnclosureNotFoundException(Guid enclosureId) 
        : base($"Enclosure with ID {enclosureId} was not found") { }
}

public class KeeperNotFoundException : DomainException
{
    public KeeperNotFoundException(Guid keeperId) 
        : base($"Keeper with ID {keeperId} was not found") { }
}

public class EnclosureIsFullException : DomainException
{
    public EnclosureIsFullException(string message) : base(message) { }
}

public class InvalidOperationException : DomainException
{
    public InvalidOperationException(string message) : base(message) { }
}

public class FeedingScheduleNotFoundException : DomainException
{
    public FeedingScheduleNotFoundException(Guid scheduleId) 
        : base($"Feeding schedule with ID {scheduleId} was not found") { }
}
