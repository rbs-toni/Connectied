using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Commands;
public record CreateGuestList : IQuery<Result<string>>
{
    public string? Name { get; set; }
    public GuestListConfigurationDto? Configuration { get; set; }
}
