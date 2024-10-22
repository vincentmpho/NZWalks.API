using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTOs.RegionDto;
using NZWalks.API.Model.DTOs.WalkDifficultyDto;
using NZWalks.API.Model.DTOs.WalkDto;
using NZWalks.API.Repositories;
using NZWalks.API.Repositories.Interface;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkDifficultiesController : ControllerBase
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var walkDifficultDomain = await _walkDifficultyRepository.GetAllAsync();

            //Convert Domain to DTO
            var wallkDifficultDTO = _mapper.Map<List<WalkDifficultyDto>>(walkDifficultDomain);

            return StatusCode(StatusCodes.Status200OK, walkDifficultDomain);
        }

        [HttpGet]
        [Route("id:guid")]
        [ActionName("GetWalkDifficultiesAync")]

        public async Task<IActionResult> GetWalkDifficultiesAync(Guid id)
        {
            var wallkDifficult = await _walkDifficultyRepository.GetAsync(id);

            //check

            if (wallkDifficult == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            //Convert Domain to DTO
            var wallkDifficultDTO = _mapper.Map<WalkDifficultyDto>(wallkDifficult);

            //return Response
            return StatusCode(StatusCodes.Status200OK, wallkDifficultDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultiesAync(AddWalkDifficultyDto addWalkDifficultyDto)
        {
            //Convert DTO to Domain
            var walkDifficultyDomain = new WalkDifficulty
            {
                Code = addWalkDifficultyDto.Code,
            };

            //Call Repository
            var walkdifficult = await _walkDifficultyRepository.AddAsync(walkDifficultyDomain);


            //Convert Domain to DTO
            var walkDifficultyDTO = _mapper.Map<WalkDifficultyDto>(walkDifficultyDomain);

            //Send DTO response back to Client
            return CreatedAtAction(nameof(GetWalkDifficultiesAync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("id:guid")]

        public async Task<IActionResult> UpdateWalkDifficulties(Guid id, [FromBody] UpdateWalkDifficultyDto updateWalkDifficultyDto)
        {
            //Convert DTO to Domain
            var walkDifficultyDomain = new WalkDifficulty
            {
                Code = updateWalkDifficultyDto.Code,
            };


            //Call repository to update 
            walkDifficultyDomain = await _walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            //check, if it's Null, then not found
            if (walkDifficultyDomain == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            //Convert Domain back to DTO

            //Convert Domain to DTO
            var walkDifficultyDTO = _mapper.Map<WalkDifficultyDto>(walkDifficultyDomain);

            //Return Ok response
            return StatusCode(StatusCodes.Status200OK, walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("id:guid")]

        public async Task<IActionResult> DeleteWalkDifficulties(Guid id)
        {
            //Call Repository to delete walk
            var walkDifficultiesDomain = await _walkDifficultyRepository.DeleteAsync(id);

            if (walkDifficultiesDomain == null)
            {
                StatusCode(StatusCodes.Status404NotFound);
            }

            var walkDifficultiesDomainDTO = _mapper.Map<WalkDifficultyDto>(walkDifficultiesDomain);

            return StatusCode(StatusCodes.Status200OK, "sucessfully deleted region");

        }
    }
}
