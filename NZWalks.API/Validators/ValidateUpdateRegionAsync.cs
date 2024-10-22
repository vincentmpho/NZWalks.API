using FluentValidation;
using NZWalks.API.Model.DTOs.RegionDto;
using NZWalks.API.Model.DTOs.WalkDifficultyDto;

namespace NZWalks.API.Validators
{
    public class ValidateUpdateRegionAsync : AbstractValidator<UpdateRegionRequestDto>
    {
        public ValidateUpdateRegionAsync()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThan(0);
            RuleFor(x => x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
