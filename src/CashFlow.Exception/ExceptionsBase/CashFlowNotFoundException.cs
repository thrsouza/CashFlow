﻿using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

public class CashFlowNotFoundException(string message) : CashFlowException(message)
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [this.Message];
    }
}