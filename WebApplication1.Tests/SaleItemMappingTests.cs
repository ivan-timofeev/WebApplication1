using AutoMapper;
using DeepEqual.Syntax;
using WebApplication1.Implementation.Helpers;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Tests;

public class SaleItemMappingTests
{
    [Fact]
    public void Map_FromSalePoint_ToSalePointVm()
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

        var salePoint = new SalePoint()
        {
            Id = Guid.NewGuid(),
            Name = "Test Sale Point"
        };

        var saleItem = new SaleItem()
        {
            ProductId = product.Id,
            Product = product,
            Quantity = 100,
            SalePointId = salePoint.Id,
            SalePoint = salePoint
        };

        // Act
        var mapped = mapper.Map<SaleItemVm>(saleItem);

        // Assert
        saleItem.WithDeepEqual(mapped)
            .IgnoreUnmatchedProperties()
            .Assert();
    }
    
    [Fact]
    public void Map_FromSalePointCreateVm_ToSalePoint()
    {
        // Arrange
        var mapper = GetMapper();
        var salePointCreateVm = AutoBogus.AutoFaker.Generate<SalePointCreateVm>();

        // Act
        var mapped = mapper.Map<SalePoint>(salePointCreateVm);

        // Assert
        mapped.WithDeepEqual(salePointCreateVm)
            .IgnoreUnmatchedProperties()
            .Assert();
    }

    private Mapper GetMapper()
    {
        return new Mapper(AutomapperConfiguration.GetAutomapperConfiguration());
    }
}