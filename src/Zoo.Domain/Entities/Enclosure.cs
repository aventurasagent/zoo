using System;
using Zoo.Domain.Exceptions;

namespace Zoo.Domain.Entities;

public class Enclosure : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public EnclosureType Type { get; private set; }
    public Capacity Capacity { get; private set; }
    public Dimensions Dimensions { get; private set; }

    private readonly List<Guid> _animalIds = new();
    public IReadOnlyCollection<Guid> AnimalIds => _animalIds.AsReadOnly();

    private Enclosure() { }

    public static Enclosure Create(string name, EnclosureType type, Capacity capacity, Dimensions dimensions)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Enclosure name cannot be empty");

        return new Enclosure
        {
            Id = Guid.NewGuid(),
            Name = name,
            Type = type,
            Capacity = capacity,
            Dimensions = dimensions
        };
    }

    public void AddAnimal(Guid animalId)
    {
        if (_animalIds.Count >= Capacity.MaxAnimals)
            throw new EnclosureIsFullException($"Enclosure {Name} is at full capacity");

        if (_animalIds.Contains(animalId))
            throw new DomainException("Animal is already in this enclosure");

        _animalIds.Add(animalId);
    }

    public void RemoveAnimal(Guid animalId)
    {
        if (!_animalIds.Contains(animalId))
            throw new DomainException("Animal is not in this enclosure");

        _animalIds.Remove(animalId);
    }

    public bool IsFull() => _animalIds.Count >= Capacity.MaxAnimals;
    public int CurrentOccupancy => _animalIds.Count;
    public int AvailableSpace => Capacity.MaxAnimals - _animalIds.Count;
}