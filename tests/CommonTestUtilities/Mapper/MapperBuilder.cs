using AutoMapper;
using CashFlow.Application.AutoMapper;

namespace CommonTestUtilities.Mapper;

public static class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(cfg =>
        { 
            cfg.AddProfile(new AutoMapping());
        }).CreateMapper();
        
        return mapper;
    }
}