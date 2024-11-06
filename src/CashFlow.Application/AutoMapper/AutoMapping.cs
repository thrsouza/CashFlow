using AutoMapper;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, configuration => configuration.Ignore());
        
        CreateMap<RequestExpenseJson, Expense>()
            .ForMember(dest => dest.Tags, configuration => configuration.MapFrom(src => src.Tags.Distinct()));

        CreateMap<TagType, Tag>()
            .ForMember(dest => dest.Value, configuration => configuration.MapFrom(src => src));
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseExpenseJson>()
            .ForMember(dest => dest.Tags, configuration => configuration.MapFrom(src => src.Tags.Select(tag => tag.Value)));
        
        CreateMap<Expense, ResponseExpensePreviewJson>();
        CreateMap<Expense, ResponseRegisteredExpenseJson>();
        
        CreateMap<User, ResponseRegisteredUserJson>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}
