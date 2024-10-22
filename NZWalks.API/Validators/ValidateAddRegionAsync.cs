using FluentValidation;
using NZWalks.API.Model.DTOs.RegionDto;

namespace NZWalks.API.Validators
{
    public class ValidateAddRegionAsync : AbstractValidator<AddRegionRequestDto>
    {
        public ValidateAddRegionAsync()
        {
            //the rules to validate the properties
            RuleFor(x => x.Code).NotEmpty(); // this is the same as ( string.isNull OR WhiteSpace()
            RuleFor(x => x.Name).NotEmpty(); 
            RuleFor(x => x.Area).GreaterThan(0); //this prop must be greter than 0
            RuleFor(x => x.Population).GreaterThanOrEqualTo(0); 

        }
    }

}
