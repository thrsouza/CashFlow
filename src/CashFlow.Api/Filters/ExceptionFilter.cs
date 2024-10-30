using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
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
            ThrowUnknownError(context);
        }
    }

    private static void HandleCashFlowException(ExceptionContext context)
    {
        var cashFlowException = (CashFlowException)context.Exception;
        context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
        var errorResponse = new ResponseErrorJson(cashFlowException.GetErrors());
        context.Result = new ObjectResult(errorResponse);
    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson([ResourceErrorMessages.UnknownError]);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
