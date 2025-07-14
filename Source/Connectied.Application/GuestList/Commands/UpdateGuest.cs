using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Commands;
public class UpdateGuest : ICommand<Result<string>>
{
    public required string Id { get; set; }
    public string? Name { get; set; }
    public string? Group { get; set; }
}
