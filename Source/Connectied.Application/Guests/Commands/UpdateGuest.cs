﻿using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Connectied.Application.Guests.Commands;
public class UpdateGuest : ICommand<Result<string>>
{
    [NotNull]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Group { get; set; }
}
