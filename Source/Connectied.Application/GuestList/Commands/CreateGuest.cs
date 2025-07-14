using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Commands;
public class CreateGuest : ICommand<Result<string>>
{
    public CreateGuest(string name)
    {
        Name = name;
    }

    public string? Group { get; set; }
    public string Name { get; set; }
}
