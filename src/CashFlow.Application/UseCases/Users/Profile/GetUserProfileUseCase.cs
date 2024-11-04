using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.AuthenticatedUser;

namespace CashFlow.Application.UseCases.Users.Profile;

public class GetUserProfileUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IMapper mapper) : IGetUserProfileUseCase
{
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await authenticatedUserService.Get();
        
        return mapper.Map<ResponseUserProfileJson>(user);
    }
}