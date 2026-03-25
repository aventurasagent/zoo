using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Commands.Animals;

public record CreateAnimalCommand(
    string Name,
    string SpeciesCommonName,
    string SpeciesScientificName,
    string SpeciesFamily,
    DietType Diet,
    DateTime BirthDate
) : ICommand<Guid>;

public class CreateAnimalCommandHandler : ICommandHandler<CreateAnimalCommand, Guid>
{
    private readonly IAnimalRepository _animalRepository;

    public CreateAnimalCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<Guid> HandleAsync(CreateAnimalCommand command, CancellationToken cancellationToken = default)
    {
        var species = new Species(
            command.SpeciesCommonName,
            command.SpeciesScientificName,
            command.SpeciesFamily,
            command.Diet
        );

        var animal = Animal.Create(command.Name, species, command.BirthDate);
        
        await _animalRepository.AddAsync(animal, cancellationToken);
        
        return animal.Id;
    }
}

public record MoveAnimalToEnclosureCommand(Guid AnimalId, Guid EnclosureId) : ICommand;

public class MoveAnimalToEnclosureCommandHandler : ICommandHandler<MoveAnimalToEnclosureCommand>
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;

    public MoveAnimalToEnclosureCommandHandler(
        IAnimalRepository animalRepository,
        IEnclosureRepository enclosureRepository)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
    }

    public async Task HandleAsync(MoveAnimalToEnclosureCommand command, CancellationToken cancellationToken = default)
    {
        var animal = await _animalRepository.GetByIdAsync(command.AnimalId, cancellationToken)
            ?? throw new AnimalNotFoundException(command.AnimalId);

        var enclosure = await _enclosureRepository.GetByIdAsync(command.EnclosureId, cancellationToken)
            ?? throw new EnclosureNotFoundException(command.EnclosureId);

        if (enclosure.IsFull())
            throw new EnclosureIsFullException($"Enclosure '{enclosure.Name}' is at full capacity");

        animal.AssignToEnclosure(command.EnclosureId);
        await _animalRepository.UpdateAsync(animal, cancellationToken);
    }
}

public record UpdateAnimalStatusCommand(Guid AnimalId, AnimalStatus NewStatus) : ICommand;

public class UpdateAnimalStatusCommandHandler : ICommandHandler<UpdateAnimalStatusCommand>
{
    private readonly IAnimalRepository _animalRepository;

    public UpdateAnimalStatusCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task HandleAsync(UpdateAnimalStatusCommand command, CancellationToken cancellationToken = default)
    {
        var animal = await _animalRepository.GetByIdAsync(command.AnimalId, cancellationToken)
            ?? throw new AnimalNotFoundException(command.AnimalId);

        switch (command.NewStatus)
        {
            case AnimalStatus.Sick:
                animal.MarkAsSick();
                break;
            case AnimalStatus.Healthy:
                animal.MarkAsHealthy();
                break;
            default:
                throw new InvalidOperationException($"Cannot directly set status to {command.NewStatus}");
        }

        await _animalRepository.UpdateAsync(animal, cancellationToken);
    }
}
