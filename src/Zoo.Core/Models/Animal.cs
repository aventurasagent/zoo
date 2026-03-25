namespace Zoo.Core;

public class Animal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Species { get; set; }
    public int Age { get; set; }
}
