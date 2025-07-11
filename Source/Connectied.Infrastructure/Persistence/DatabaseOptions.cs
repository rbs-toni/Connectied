using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Connectied.Infrastructure.Persistence;
public class DatabaseOptions : IValidatableObject
{
    public string? Provider { get; set; }
    public string? ConnectionString { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            yield return new ValidationResult("Connection string is empty.");
        }
    }
}
