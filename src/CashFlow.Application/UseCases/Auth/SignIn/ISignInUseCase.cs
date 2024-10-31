using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Auth.SignIn;

public interface ISignInUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestSignInJson request);
}