namespace Zoo.Domain.ValueObjects;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}

public class Species : ValueObject
{
    public string CommonName { get; }
    public string ScientificName { get; }
    public string Family { get; }
    public DietType Diet { get; }

    public Species(string commonName, string scientificName, string family, DietType diet)
    {
        CommonName = commonName ?? throw new ArgumentNullException(nameof(commonName));
        ScientificName = scientificName ?? throw new ArgumentNullException(nameof(scientificName));
        Family = family ?? throw new ArgumentNullException(nameof(family));
        Diet = diet;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ScientificName.ToLowerInvariant();
    }

    public override string ToString() => $"{CommonName} ({ScientificName})";
}

public enum DietType
{
    Carnivore,
    Herbivore,
    Omnivore,
    Insectivore,
    Piscivore
}

public class Capacity : ValueObject
{
    public int MaxAnimals { get; }
    public int? MaxWeight { get; } // in kg

    public Capacity(int maxAnimals, int? maxWeight = null)
    {
        if (maxAnimals <= 0)
            throw new ArgumentException("Max animals must be greater than 0", nameof(maxAnimals));
        
        MaxAnimals = maxAnimals;
        MaxWeight = maxWeight;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MaxAnimals;
        yield return MaxWeight ?? 0;
    }
}

public class Dimensions : ValueObject
{
    public decimal Length { get; } // meters
    public decimal Width { get; }  // meters
    public decimal Height { get; } // meters
    
    public decimal Area => Length * Width;
    public decimal Volume => Length * Width * Height;

    public Dimensions(decimal length, decimal width, decimal height)
    {
        if (length <= 0) throw new ArgumentException("Length must be positive", nameof(length));
        if (width <= 0) throw new ArgumentException("Width must be positive", nameof(width));
        if (height <= 0) throw new ArgumentException("Height must be positive", nameof(height));
        
        Length = length;
        Width = width;
        Height = height;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Length;
        yield return Width;
        yield return Height;
    }

    public override string ToString() => $"{Length}m x {Width}m x {Height}m";
}

public class Email : ValueObject
{
    public string Address { get; }

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Email cannot be empty", nameof(address));
        
        if (!address.Contains('@'))
            throw new ArgumentException("Invalid email format", nameof(address));
            
        Address = address.ToLowerInvariant();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }

    public override string ToString() => Address;
}

public class PhoneNumber : ValueObject
{
    public string Number { get; }
    public string? CountryCode { get; }

    public PhoneNumber(string number, string? countryCode = null)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Phone number cannot be empty", nameof(number));
            
        Number = number;
        CountryCode = countryCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
        yield return CountryCode ?? string.Empty;
    }

    public override string ToString() => $"{CountryCode} {Number}".Trim();
}
