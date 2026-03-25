using System;
using Zoo.Domain.Exceptions;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Entities;

public class Animal : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Species Species { get; private set; }
    public DateTime BirthDate { get; private set; }
    public AnimalStatus Status { get; private set; }
    public Guid? EnclosureId { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Animal() { }

    public static Animal Create(string name, Species species, DateTime birthDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Animal name cannot be empty");

        return new Animal
        {
            Id = Guid.NewGuid(),
            Name = name,
            Species = species,
            BirthDate = birthDate,
            Status = AnimalStatus.Healthy
        };
    }

    public void AssignToEnclosure(Guid enclosureId)
    {
        EnclosureId = enclosureId;
        _domainEvents.Add(new AnimalMovedToEnclosure(Id, enclosureId));
    }

    public void MarkAsSick()
    {
        if (Status == AnimalStatus.Sick)
            throw new DomainException("Animal is already sick");

        Status = AnimalStatus.Sick;
        _domainEvents.Add(new AnimalStatusChanged(Id, Status));
    }

    public void MarkAsHealthy()
    {
        if (Status == AnimalStatus.Healthy)
            throw new DomainException("Animal is already healthy");

        Status = AnimalStatus.Healthy;
        _domainEvents.Add(new AnimalStatusChanged(Id, Status));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

public enum AnimalStatus
{
    Healthy,
    Sick,
    UnderTreatment,
    Quarantine
}