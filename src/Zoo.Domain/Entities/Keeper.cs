using Zoo.Domain.Exceptions;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Entities;

public class Keeper : IEntity
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public DateTime HireDate { get; private set; }
    public KeeperStatus Status { get; private set; }
    
    private readonly List<Guid> _assignedEnclosureIds = new();
    public IReadOnlyCollection<Guid> AssignedEnclosureIds => _assignedEnclosureIds.AsReadOnly();
    
    private Keeper() { }
    
    public static Keeper Create(string firstName, string lastName, Email email, PhoneNumber phone)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty");
            
        return new Keeper
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            HireDate = DateTime.UtcNow,
            Status = KeeperStatus.Active
        };
    }
    
    public void AssignToEnclosure(Guid enclosureId)
    {
        if (_assignedEnclosureIds.Contains(enclosureId))
            throw new DomainException("Keeper is already assigned to this enclosure");
            
        _assignedEnclosureIds.Add(enclosureId);
    }
    
    public void RemoveFromEnclosure(Guid enclosureId)
    {
        if (!_assignedEnclosureIds.Contains(enclosureId))
            throw new DomainException("Keeper is not assigned to this enclosure");
            
        _assignedEnclosureIds.Remove(enclosureId);
    }
    
    public void Deactivate()
    {
        Status = KeeperStatus.Inactive;
    }
    
    public void Activate()
    {
        Status = KeeperStatus.Active;
    }
    
    public string FullName => $"{FirstName} {LastName}";
}

public enum KeeperStatus
{
    Active,
    Inactive,
    OnLeave
}
