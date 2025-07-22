using Connectied.Domain.Guests;
using FluentValidation;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Commands;
class CreateGuestValidator : AbstractValidator<Guest>
{
    public CreateGuestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(50);
    }
}
