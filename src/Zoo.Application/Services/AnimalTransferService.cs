using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Services;

public class AnimalTransferService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AnimalTransferService(
        IAnimalRepository animalRepository,
        IEnclosureRepository enclosureRepository,
        IUnitOfWork unitOfWork)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task TransferAnimalAsync(
        Guid animalId, 
        Guid targetEnclosureId, 
        CancellationToken cancellationToken = default)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId, cancellationToken)
            ?? throw new AnimalNotFoundException(animalId);

        var enclosure = await _enclosureRepository.GetByIdAsync(targetEnclosureId, cancellationToken)
            ?? throw new EnclosureNotFoundException(targetEnclosureId);

        // Business rule: Check if enclosure is full
        if (enclosure.IsFull())
            throw new EnclosureIsFullException($"Cannot transfer animal. Enclosure '{enclosure.Name}' is at full capacity.");

        // Remove from current enclosure if assigned
        if (animal.EnclosureId.HasValue)
        {
            var currentEnclosure = await _enclosureRepository.GetByIdAsync(animal.EnclosureId.Value, cancellationToken);
            currentEnclosure?.RemoveAnimal(animalId);
            if (currentEnclosure != null)
                await _enclosureRepository.UpdateAsync(currentEnclosure, cancellationToken);
        }

        // Add to new enclosure
        enclosure.AddAnimal(animalId);
        animal.AssignToEnclosure(targetEnclosureId);

        await _animalRepository.UpdateAsync(animal, cancellationToken);
        await _enclosureRepository.UpdateAsync(enclosure, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
