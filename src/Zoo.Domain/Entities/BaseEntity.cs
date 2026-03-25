namespace Zoo.Domain;

public interface IEntity
{
    Guid Id { get; }
}

public interface IAggregateRoot : IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
