﻿using System.ComponentModel.DataAnnotations;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Web.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    // private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
        // Register known exception types and handlers.
        // _exceptionHandlers = new()
        // {
        //     { typeof(ValidationException), HandleValidationException },
        //     // { typeof(NotFoundException), HandleNotFoundException },
        //     { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
        //     { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
        //     
        // };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        // var exceptionType = exception.GetType();
        //
        // if (_exceptionHandlers.ContainsKey(exceptionType))
        // {
        //     await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
        //     return true;
        // }
        //
        // return false;
        
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = exception.Message
        };
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    // private async Task HandleValidationException(HttpContext httpContext, Exception ex)
    // {
    //     var exception = (ValidationException)ex;
    //
    //     httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    //
    //     await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
    //     {
    //         Status = StatusCodes.Status400BadRequest,
    //         Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
    //     });
    // }

    // private async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
    // {
    //     var exception = (NotFoundException)ex;
    //
    //     httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
    //
    //     await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
    //     {
    //         Status = StatusCodes.Status404NotFound,
    //         Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    //         Title = "The specified resource was not found.",
    //         Detail = exception.Message
    //     });
    // }

    // private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    // {
    //     httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //
    //     await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
    //     {
    //         Status = StatusCodes.Status401Unauthorized,
    //         Title = "Unauthorized",
    //         Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
    //     });
    // }

    // private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    // {
    //     httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
    //
    //     await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
    //     {
    //         Status = StatusCodes.Status403Forbidden,
    //         Title = "Forbidden",
    //         Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
    //     });
    // }
}