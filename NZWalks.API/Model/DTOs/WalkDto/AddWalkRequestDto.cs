using NZWalks.API.Model.Domain;

namespace NZWalks.API.Model.DTOs.WalkDto
{
    public class AddWalkRequestDto
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid WalkDifficultyId { get; set; }
        public Guid RegionId { get; set; }

    }
}
