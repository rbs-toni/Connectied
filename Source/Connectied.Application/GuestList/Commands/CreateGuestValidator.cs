using Connectied.Domain.GuestList;
using FluentValidation;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Commands;
class CreateGuestValidator : AbstractValidator<Guest>
{
    public CreateGuestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(50);
    }
}
