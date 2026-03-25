using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Commands.Keepers;

public record CreateKeeperCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? CountryCode = null
) : ICommand<Guid>;

public class CreateKeeperCommandHandler : ICommandHandler<CreateKeeperCommand, Guid>
{
    private readonly IKeeperRepository _keeperRepository;

    public CreateKeeperCommandHandler(IKeeperRepository keeperRepository)
    {
        _keeperRepository = keeperRepository;
    }

    public async Task<Guid> HandleAsync(CreateKeeperCommand command, CancellationToken cancellationToken = default)
    {
        var email = new Email(command.Email);
        var phone = new PhoneNumber(command.PhoneNumber, command.CountryCode);

        var keeper = Keeper.Create(command.FirstName, command.LastName, email, phone);
        
        await _keeperRepository.AddAsync(keeper, cancellationToken);
        
        return keeper.Id;
    }
}

public record AssignKeeperToEnclosureCommand(Guid KeeperId, Guid EnclosureId) : ICommand;

public class AssignKeeperToEnclosureCommandHandler : ICommandHandler<AssignKeeperToEnclosureCommand>
{
    private readonly IKeeperRepository _keeperRepository;

    public AssignKeeperToEnclosureCommandHandler(IKeeperRepository keeperRepository)
    {
        _keeperRepository = keeperRepository;
    }

    public async Task HandleAsync(AssignKeeperToEnclosureCommand command, CancellationToken cancellationToken = default)
    {
        var keeper = await _keeperRepository.GetByIdAsync(command.KeeperId, cancellationToken)
            ?? throw new KeeperNotFoundException(command.KeeperId);

        keeper.AssignToEnclosure(command.EnclosureId);
        
        await _keeperRepository.UpdateAsync(keeper, cancellationToken);
    }
}
