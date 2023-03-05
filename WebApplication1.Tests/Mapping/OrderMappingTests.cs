using AutoMapper;
using DeepEqual.Syntax;
using WebApplication1.Implementation.Helpers;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Tests;

public class OrderMappingTests
{
    [Fact]
    public void Map_FromProduct_ToProductVm()
    {
        // Arrange
        var mapper = GetMapper();


        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = "IPhone 14 Pro",
            Description = "RU, 256GB, GRAY",
            ProductCharacteristics = new List<ProductCharacteristic>()
            {
                new StringProductCharacteristic()
                {
                    Name = "Color",
                    Value = "Gray"
                },
                new NumberProductCharacteristic()
                {
                    Name = "StorageGB",
                    Value = 256
                }
            }
        };

        var salePoint = new SalePoint()
        {
            Name = "Тестовая торговая точка",
            Address = "ул.Боровицкая, Кремль",
            SaleItems = new List<SaleItem>()
            {
                new SaleItem()
                {
                    Product = product,
                    Quantity = 1000,
                    PurchasePrice = 85000
                }
            }
        };

        var order = new Order()
        {
            OrderStateHierarchical = new List<OrderStateHierarchicalItem>()
            {
                new OrderStateHierarchicalItem()
                {
                    SerialNumber = 1,
                    EnteredDateTimeUtc = DateTime.UtcNow,
                    State = OrderStateEnum.Creating,
                    Details = "Пользователь начал выбор продуктов"
                },
                new OrderStateHierarchicalItem()
                {
                    SerialNumber = 2,
                    EnteredDateTimeUtc = DateTime.UtcNow,
                    State = OrderStateEnum.AwaitingAssembling,
                    Details = "Заказ сформирован и зарезервирован. Ожидание сборщика."
                }
            },
            OrderedItems = new List<OrderItem>()
            {
                new OrderItem()
                {
                    SaleItem = new SaleItem()
                    {
                        SalePoint = salePoint,
                        Product = product,
                        SellingPrice = 80000
                    },
                    Quantity = 1,
                    Price = 80000
                }
            }
        };


        // Act
        var mapped = mapper.Map<OrderVm>(order);

        // Assert
        order.WithDeepEqual(mapped)
            .IgnoreUnmatchedProperties()
            .Assert();
    }

    private Mapper GetMapper()
    {
        return new Mapper(AutomapperConfiguration.GetAutomapperConfiguration());
    }
}