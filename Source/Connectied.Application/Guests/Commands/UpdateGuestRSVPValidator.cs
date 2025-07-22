using FluentValidation;

namespace Connectied.Application.Guests.Commands;

sealed class UpdateGuestRSVPValidator : AbstractValidator<UpdateGuestRSVP>
{
    public UpdateGuestRSVPValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Event1Status)
            .IsInEnum().WithMessage("Event 1 RSVP status must be a valid enum value");
        RuleFor(x => x.Event2Status)
            .IsInEnum().WithMessage("Event 2 RSVP status must be a valid enum value");
    }
}
