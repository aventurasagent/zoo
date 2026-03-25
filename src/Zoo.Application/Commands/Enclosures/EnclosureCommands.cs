using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Commands.Enclosures;

public record CreateEnclosureCommand(
    string Name,
    EnclosureType Type,
    int MaxAnimals,
    int? MaxWeight,
    decimal Length,
    decimal Width,
    decimal Height
) : ICommand<Guid>;

public class CreateEnclosureCommandHandler : ICommandHandler<CreateEnclosureCommand, Guid>
{
    private readonly IEnclosureRepository _enclosureRepository;

    public CreateEnclosureCommandHandler(IEnclosureRepository enclosureRepository)
    {
        _enclosureRepository = enclosureRepository;
    }

    public async Task<Guid> HandleAsync(CreateEnclosureCommand command, CancellationToken cancellationToken = default)
    {
        var capacity = new Capacity(command.MaxAnimals, command.MaxWeight);
        var dimensions = new Dimensions(command.Length, command.Width, command.Height);

        var enclosure = Enclosure.Create(command.Name, command.Type, capacity, dimensions);
        
        await _enclosureRepository.AddAsync(enclosure, cancellationToken);
        
        return enclosure.Id;
    }
}

public record AddAnimalToEnclosureCommand(Guid EnclosureId, Guid AnimalId) : ICommand;

public class AddAnimalToEnclosureCommandHandler : ICommandHandler<AddAnimalToEnclosureCommand>
{
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IAnimalRepository _animalRepository;

    public AddAnimalToEnclosureCommandHandler(
        IEnclosureRepository enclosureRepository,
        IAnimalRepository animalRepository)
    {
        _enclosureRepository = enclosureRepository;
        _animalRepository = animalRepository;
    }

    public async Task HandleAsync(AddAnimalToEnclosureCommand command, CancellationToken cancellationToken = default)
    {
        var enclosure = await _enclosureRepository.GetByIdAsync(command.EnclosureId, cancellationToken)
            ?? throw new EnclosureNotFoundException(command.EnclosureId);

        var animal = await _animalRepository.GetByIdAsync(command.AnimalId, cancellationToken)
            ?? throw new AnimalNotFoundException(command.AnimalId);

        enclosure.AddAnimal(command.AnimalId);
        animal.AssignToEnclosure(command.EnclosureId);

        await _enclosureRepository.UpdateAsync(enclosure, cancellationToken);
        await _animalRepository.UpdateAsync(animal, cancellationToken);
    }
}
