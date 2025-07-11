using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Connectied.Application.Common.Exceptions;
[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "<Pending>")]
public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
