using FluentValidation;
using NZWalks.API.Model.DTOs.WalkDto;

namespace NZWalks.API.Validators
{
    public class UpdateWalkRequestValidator : AbstractValidator<AddWalkRequestDto>
    {
        public UpdateWalkRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
