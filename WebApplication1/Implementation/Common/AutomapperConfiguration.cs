using AutoMapper;
using WebApplication1.Models;
using WebApplication1.ViewModels;
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
    }

    public static IServiceCollection AddConfiguredAutoMapper(this IServiceCollection services)
    {
        services.AddTransient<IMapper, Mapper>(x => new Mapper(GetAutomapperConfiguration()));
        return services;
    }
}
