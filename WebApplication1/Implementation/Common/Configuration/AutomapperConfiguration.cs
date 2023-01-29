using AutoMapper;
using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.Customer;
using WebApplication1.ViewModels.Manufacturer;

namespace WebApplication1.Implementation.Helpers;

public static class AutomapperConfiguration
{
    public static MapperConfiguration GetAutomapperConfiguration()
    {
        return new MapperConfiguration(cfg =>
        {
            ConfigureModelToViewModel(cfg);
            ConfigureViewModelToModel(cfg);
        });
    }

    private static void ConfigureModelToViewModel(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ProductCharacteristic, ProductCharacteristicVm>()
            .Include<StringProductCharacteristic, StringProductCharacteristicVm>()
            .Include<NumberProductCharacteristic, NumberProductCharacteristicVm>();
        cfg.CreateMap<StringProductCharacteristic, StringProductCharacteristicVm>();
        cfg.CreateMap<NumberProductCharacteristic, NumberProductCharacteristicVm>();
        cfg.CreateMap<Product, ProductVm>();

        cfg.CreateMap<Manufacturer, ManufacturerVm>();

        cfg.CreateMap<SaleItem, SaleItemVm>()
            .ReverseMap();
        cfg.CreateMap<SalePoint, SalePointVm>()
            .ReverseMap();

        cfg.CreateMap<SalePointCreateVm, SalePoint>();
        cfg.CreateMap<SaleItemCreateVm, SaleItem>();

        cfg.CreateMap<SalePointUpdateVm, SalePoint>();
        cfg.CreateMap<SaleItemUpdateVm, SaleItem>();

        cfg.CreateMap<Customer, CustomerVm>();

        cfg.CreateMap<Order, OrderVm>();
        cfg.CreateMap<OrderStateHierarchicalItem, OrderStateHierarchicalItemVm>();
    }
    
    private static void ConfigureViewModelToModel(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ProductCharacteristicVm, ProductCharacteristic>()
            .Include<StringProductCharacteristicVm, StringProductCharacteristic>()
            .Include<NumberProductCharacteristicVm, NumberProductCharacteristic>();
        cfg.CreateMap<StringProductCharacteristicVm, StringProductCharacteristic>();
        cfg.CreateMap<NumberProductCharacteristicVm, NumberProductCharacteristic>();
        cfg.CreateMap<ProductVm, Product>();
        cfg.CreateMap<ProductCreateVm, Product>();
        cfg.CreateMap<ProductUpdateVm, Product>();

        cfg.CreateMap<ManufacturerCreateVm, Manufacturer>();
        cfg.CreateMap<ManufacturerUpdateVm, Manufacturer>();

        cfg.CreateMap<CustomerCreateVm, Customer>();
        cfg.CreateMap<CustomerUpdateVm, Customer>();
    }

    public static IServiceCollection AddConfiguredAutoMapper(this IServiceCollection services)
    {
        services.AddTransient<IMapper, Mapper>(x => new Mapper(GetAutomapperConfiguration()));
        return services;
    }
}