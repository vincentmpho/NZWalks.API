namespace NZWalks.API.Model.DTOs.WalkDto
{
    public class UpdateWalkRequestDto
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid WalkDifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
