using Zoo.Core;

namespace Zoo.Tests;

public class AnimalTests
{
    [Fact]
    public void Animal_HasRequiredProperties()
    {
        // Arrange & Act
        var animal = new Animal
        {
            Name = "Leo",
            Species = "Lion",
            Age = 5
        };

        // Assert
        Assert.NotEqual(Guid.Empty, animal.Id);
        Assert.Equal("Leo", animal.Name);
        Assert.Equal("Lion", animal.Species);
        Assert.Equal(5, animal.Age);
    }

    [Fact]
    public void Animal_GeneratesUniqueId()
    {
        // Arrange & Act
        var animal1 = new Animal { Name = "A", Species = "Test" };
        var animal2 = new Animal { Name = "B", Species = "Test" };

        // Assert
        Assert.NotEqual(animal1.Id, animal2.Id);
    }
}
