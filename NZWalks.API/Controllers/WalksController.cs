using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTOs.WalkDto;
using NZWalks.API.Repositories.Interface;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        private readonly IRegionRepository _regionRepository;
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository,
            IWalkDifficultyRepository walkDifficultyRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
            _walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch Data From Database- Domain walks
            var walksDomain = await _walkRepository.GetAllAsync();

            //Convert Domain Walks to DTO Walks
            var walksDTO = _mapper.Map<List<WalkDto>>(walksDomain);
            //Retrun Response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk Domain object from database
            var walkdomain = await _walkRepository.GetAsync(id);

            //convert Domain object to DTO
            var walkDTO = _mapper.Map<WalkDto>(walkdomain);

            //Return response
            return Ok(walkDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequestDto addWalkRequestDto)
        {

            //Validate the incoming request
            if (! await ValidateAddWalkAsync(addWalkRequestDto))
            {
                return BadRequest(ModelState);
            }
          
            //Convert DTO to Domain Object

            var walkDomain = new Walk
            {
                Length = addWalkRequestDto.Length,
                Name = addWalkRequestDto.Name,
                RegionId = addWalkRequestDto.RegionId,
                WalkDifficultyId = addWalkRequestDto.WalkDifficultyId,
            };

            //Pass domain object to reposioty to perist this
            walkDomain = await _walkRepository.AddAsync(walkDomain);

            //Convert the Domain object back to DTO

            var walkDTO = new WalkDto
            {
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };

            //Send DTO response back to Client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }


        //latest
        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Convert DTO to domain object
            var walkDomain = new Walk
            {
                Length = updateWalkRequestDto.Length,
                Name = updateWalkRequestDto.Name,
                RegionId = updateWalkRequestDto.RegionId,
                WalkDifficultyId = updateWalkRequestDto.WalkDifficultyId,
            };

            //Pass Details to Reposioty- Get Domain object in response (or null)
            walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

            //Handle Null (not null)
            if (walkDomain == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "walk with this id not found");
            }

            //Convert back Domain to DTO

            //var walkDTO = new WalkDto
            //{
            //    Length = walkDomain.Length,
            //    Name = walkDomain.Name,
            //    RegionId = walkDomain.RegionId,
            //    WalkDifficultyId = walkDomain.WalkDifficultyId,
            //};

            //use Mapper
            var walkDTO = _mapper.Map<WalkDto>(walkDomain);


            //Return Response
            return StatusCode(StatusCodes.Status200OK, walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Call Repository to delete walk
            var walkDomain = await _walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                StatusCode(StatusCodes.Status404NotFound);
            }

            var walkDTO = _mapper.Map<WalkDto>(walkDomain);

            return StatusCode(StatusCodes.Status200OK, walkDTO);
        }

        #region Private methods
        private  async Task<bool> ValidateAddWalkAsync(AddWalkRequestDto addWalkRequestDto)
        {
            //    if (addWalkRequestDto == null)
            //    {
            //        ModelState.AddModelError(nameof(AddWalkAsync),
            //            $"{nameof(addWalkRequestDto)} cannot be  empty.");
            //        return false;
            //    }

            //    if (!string.IsNullOrWhiteSpace(addWalkRequestDto.Name))
            //    {
            //        ModelState.AddModelError(nameof(addWalkRequestDto.Name),
            //            $"{nameof(addWalkRequestDto.Name)} is requred");
            //    }

            //    if (addWalkRequestDto.Length>0)
            //    {
            //        ModelState.AddModelError(nameof(addWalkRequestDto.Length),
            //            $"{nameof(addWalkRequestDto.Length)} should be greater than zero");
            //    }

            var region = _regionRepository.GetAsync(addWalkRequestDto.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequestDto.RegionId),
                    $"{nameof(addWalkRequestDto.RegionId)} is invalid");
            }

            //check if walkdifficulty it's exist

            var walkDifificuly = await _walkDifficultyRepository.GetAsync(addWalkRequestDto.WalkDifficultyId);

            if(walkDifificuly == null)
            {
                ModelState.AddModelError(nameof(addWalkRequestDto.WalkDifficultyId),
                    $"{nameof(addWalkRequestDto.WalkDifficultyId)} is invalid");
            }
            //check

            if (ModelState.ErrorCount >0)
            {
                return false;                     
            }
            return true;
        }
        #endregion
    }
}
