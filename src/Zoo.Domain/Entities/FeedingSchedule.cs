using Zoo.Domain.Exceptions;

namespace Zoo.Domain.Entities;

public class FeedingSchedule : IEntity
{
    public Guid Id { get; private set; }
    public Guid AnimalId { get; private set; }
    public FeedingTime FeedingTime { get; private set; }
    public FoodType FoodType { get; private set; }
    public Quantity Quantity { get; private set; }
    public List<DayOfWeek> Days { get; private set; }
    public string? SpecialInstructions { get; private set; }
    public bool IsActive { get; private set; }
    
    private FeedingSchedule() { }
    
    public static FeedingSchedule Create(
        Guid animalId, 
        FeedingTime feedingTime, 
        FoodType foodType, 
        Quantity quantity,
        IEnumerable<DayOfWeek> days,
        string? specialInstructions = null)
    {
        var daysList = days.ToList();
        if (!daysList.Any())
            throw new DomainException("At least one day must be specified");
            
        return new FeedingSchedule
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            FeedingTime = feedingTime,
            FoodType = foodType,
            Quantity = quantity,
            Days = daysList,
            SpecialInstructions = specialInstructions,
            IsActive = true
        };
    }
    
    public void UpdateSchedule(FeedingTime newTime, IEnumerable<DayOfWeek> newDays)
    {
        FeedingTime = newTime;
        Days = newDays.ToList();
    }
    
    public void UpdateFood(FoodType newFoodType, Quantity newQuantity)
    {
        FoodType = newFoodType;
        Quantity = newQuantity;
    }
    
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
    
    public bool IsScheduledFor(DateTime date) => Days.Contains(date.DayOfWeek) && IsActive;
}

public record FeedingTime(int Hour, int Minute)
{
    public TimeOnly ToTimeOnly() => new TimeOnly(Hour, Minute);
    public override string ToString() => $"{Hour:D2}:{Minute:D2}";
}

public enum FoodType
{
    Meat,
    Vegetables,
    Fruits,
    Fish,
    Insects,
    Seeds,
    Hay,
    Specialized
}

public record Quantity(decimal Amount, string Unit)
{
    public override string ToString() => $"{Amount} {Unit}";
}
