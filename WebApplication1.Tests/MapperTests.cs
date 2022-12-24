using AutoMapper;
using DeepEqual.Syntax;
using WebApplication1.Implementation.Helpers;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Tests;

public class MapperTests
{
    [Fact]
    public void TestMapping_Product_To_ProductVm()
    {
        // Arrange
        var mapper = GetMapper();
        
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Car",
            Description = "Wow, its so nice car",
            ProductCharacteristics = new List<ProductCharacteristic>()
            {
                new NumberProductCharacteristic()
                {
                    Id = Guid.NewGuid(),
                    Name = "Horses count",
                    Value = 160
                },
                new StringProductCharacteristic()
                {
                    Id = Guid.NewGuid(),
                    Name = "Color",
                    Value = "Black"
                }
            }
        };

        // Act
        var mapped = mapper.Map<ProductVm>(product);

        // Assert
        product.WithDeepEqual(mapped)
            .IgnoreUnmatchedProperties()
            .Assert();
    }

    private Mapper GetMapper()
    {
        return new Mapper(AutomapperConfiguration.GetAutomapperConfiguration());
    }
}