using AutoMapper;
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
        CreateMap<RequestRegisterExpenseJson, Expense>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());

    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseExpenseJson>();
        CreateMap<Expense, ResponseExpensePreviewJson>();
        CreateMap<Expense, ResponseRegisteredExpenseJson>();
        CreateMap<User, ResponseRegisteredUserJson>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}
