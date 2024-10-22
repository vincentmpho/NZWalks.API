using FluentValidation;
using NZWalks.API.Model.DTOs.WalkDifficultyDto;

namespace NZWalks.API.Validators
{
    public class AddWalkDifficultyRequestValidator : AbstractValidator<AddWalkDifficultyDto>
    {
        public AddWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
