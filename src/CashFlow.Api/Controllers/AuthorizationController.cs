using CashFlow.Application.UseCases.Auth.SignIn;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ResponseRegisteredUserJson>(StatusCodes.Status200OK)]
    [ProducesResponseType<ResponseErrorJson>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn(
        [FromServices] ISignInUseCase useCase,
        [FromBody] RequestAuthorizationJson request)
    {
        var response = await useCase.Execute(request);
        
        return Ok(response);
    }
}