using CashFlow.Communication.Responses;
using CashFlow.Exeption;
using CashFlow.Exeption.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CashFlowException)
        {
            HandleCashFlowException(context);
        } 
        else
        {
            ThrowUnkownError(context);
        }
    }

    private void HandleCashFlowException(ExceptionContext context)
    {
        var cashFlowException = (CashFlowException)context.Exception;
        context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
        var errorResponse = new ResponseErrorJson(cashFlowException.GetErrors());
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnkownError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson([ResourceErrorMessages.UNKNOWN_ERROR]);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
