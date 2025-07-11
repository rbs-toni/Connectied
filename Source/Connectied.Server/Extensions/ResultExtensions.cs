using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Connectied.Server.Extensions;
public static class ResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult<T>(this Result<T> result)
    {
        return ((Ardalis.Result.IResult)result).ToMinimalApiResult();
    }
    public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult(this Result result)
    {
        return ((Ardalis.Result.IResult)result).ToMinimalApiResult();
    }

    internal static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult(this Ardalis.Result.IResult result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => (result is Result) ? Results.Ok() : Results.Ok(result.GetValue()),
            ResultStatus.Created => Results.Created(string.Empty, result.GetValue()),
            ResultStatus.NoContent => Results.NoContent(),
            ResultStatus.NotFound => NotFoundEntity(result),
            ResultStatus.Unauthorized => UnAuthorized(result),
            ResultStatus.Forbidden => Forbidden(result),
            ResultStatus.Invalid => Results.BadRequest(result.ValidationErrors),
            ResultStatus.Error => UnprocessableEntity(result),
            ResultStatus.Conflict => ConflictEntity(result),
            ResultStatus.Unavailable => UnavailableEntity(result),
            ResultStatus.CriticalError => CriticalEntity(result),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }
    static Microsoft.AspNetCore.Http.IResult ConflictEntity(Ardalis.Result.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (string error in result.Errors)
            {
                stringBuilder.Append("* ").AppendLine(error);
            }

            return Results.Conflict(new ProblemDetails
            {
                Title = "There was a conflict.",
                Detail = stringBuilder.ToString()
            });
        }

        return Results.Conflict();
    }
    static Microsoft.AspNetCore.Http.IResult CriticalEntity(Ardalis.Result.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (string error in result.Errors)
            {
                stringBuilder.Append("* ").AppendLine(error);
            }

            return Results.Problem(new ProblemDetails
            {
                Title = "Something went wrong.",
                Detail = stringBuilder.ToString(),
                Status = 500
            });
        }

        return Results.StatusCode(500);
    }
    static Microsoft.AspNetCore.Http.IResult Forbidden(Ardalis.Result.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (string error in result.Errors)
            {
                stringBuilder.Append("* ").AppendLine(error);
            }

            return Results.Problem(new ProblemDetails
            {
                Title = "Forbidden.",
                Detail = stringBuilder.ToString(),
                Status = 403
            });
        }

        return Results.Forbid();
    }
    static Microsoft.AspNetCore.Http.IResult NotFoundEntity(Ardalis.Result.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (string error in result.Errors)
            {
                stringBuilder.Append("* ").AppendLine(error);
            }

            return Results.NotFound(new ProblemDetails
            {
                Title = "Resource not found.",
                Detail = stringBuilder.ToString(),
            });
        }

        return Results.NotFound();
    }
    static Microsoft.AspNetCore.Http.IResult UnAuthorized(Ardalis.Result.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (string error in result.Errors)
            {
                stringBuilder.Append("* ").AppendLine(error);
            }

            return Results.Problem(new ProblemDetails
            {
                Title = "Unauthorized.",
                Detail = stringBuilder.ToString(),
                Status = 401
            });
        }

        return Results.Unauthorized();
    }
    static Microsoft.AspNetCore.Http.IResult UnavailableEntity(Ardalis.Result.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (string error in result.Errors)
            {
                stringBuilder.Append("* ").AppendLine(error);
            }

            return Results.Problem(new ProblemDetails
            {
                Title = "Service unavailable.",
                Detail = stringBuilder.ToString(),
                Status = 503
            });
        }

        return Results.StatusCode(503);
    }
    static Microsoft.AspNetCore.Http.IResult UnprocessableEntity(Ardalis.Result.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        foreach (string error in result.Errors)
        {
            stringBuilder.Append("* ").AppendLine(error);
        }

        return Results.UnprocessableEntity(new ProblemDetails
        {
            Title = "Something went wrong.",
            Detail = stringBuilder.ToString()
        });
    }
}
