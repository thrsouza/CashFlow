using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Auth.SignIn;

public class SignInUseCase(
    IUsersReadOnlyRepository repository,
    IPasswordEncryptor passwordEncryptor,
    IAccessTokenGenerator accessTokenGenerator)
    : ISignInUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestAuthorizationJson request)
    {
        var user = await repository.GetByEmail(request.Email);

        if (user is not null && passwordEncryptor.Verify(request.Password, user.Password))
        {
            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Token = accessTokenGenerator.Generate(user)
            };
        }

        throw new CashFlowInvalidLoginException();
    }
}