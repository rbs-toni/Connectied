using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Commands;
public record CreateGuest : ICommand<Result<string>>
{
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Group { get; set; }
    public string? Parent { get; set; }
    public int Event1Quota { get; set; }
    public int Event2Quota { get; set; }
}
