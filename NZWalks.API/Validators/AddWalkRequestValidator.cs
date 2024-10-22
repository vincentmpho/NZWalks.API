using FluentValidation;
using NZWalks.API.Model.DTOs.WalkDto;

namespace NZWalks.API.Validators
{
    public class AddWalkRequestValidator : AbstractValidator<AddWalkRequestDto>
    {
        public AddWalkRequestValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Length).GreaterThan(0);
           
        }
    }
}
