﻿using System;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.Common.Behaviours;
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger) { _logger = logger; }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(
                ex,
                "Connectied Request: Unhandled Exception for Request {Name} {@Request}",
                requestName,
                request);

            throw;
        }
    }
}
