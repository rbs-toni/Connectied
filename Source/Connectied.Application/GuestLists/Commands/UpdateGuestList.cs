using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Commands;
public record UpdateGuestList : ICommand<Result<string>>
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public GuestListConfigurationDto? Configuration { get; set; }
}
