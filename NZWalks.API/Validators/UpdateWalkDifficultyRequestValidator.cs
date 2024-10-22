using FluentValidation;
using NZWalks.API.Model.DTOs.WalkDifficultyDto;

namespace NZWalks.API.Validators
{
    public class UpdateWalkDifficultyRequestValidator : AbstractValidator<AddWalkDifficultyDto>
    {
        public UpdateWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
