using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTOs.RegionDto;
using NZWalks.API.Repositories.Interface;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async  Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();

            //return DTO regions
            //var regiondto = new List<RegionDto>();

            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new RegionDto()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regiondto.Add(regionDTO);
            //});

              var regiondto = _mapper.Map<List<RegionDto>>(regions);

            return Ok(regiondto);
        } 

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionsAsync")]
        public async Task<IActionResult> GetRegionsAsync(Guid id)
        {
          var domainRegion =  await _regionRepository.GetAsync(id);

            if(domainRegion == null)
            {
                return StatusCode(StatusCodes.Status404NotFound," INVALID ID");
            }
          var regiondto=  _mapper.Map<RegionDto>(domainRegion);



            return StatusCode(StatusCodes.Status200OK, regiondto);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequestDto addRegionRequestDto)
        {
            //Validste the request ( addRegion)
             if (!ValidateAddRegionAsync(addRegionRequestDto))
            {
                return BadRequest(ModelState);
            }



            //Request to Domain Model
            var region = new Region()
            {
                Code = addRegionRequestDto.Code,
                Area = addRegionRequestDto.Area,
                Lat = addRegionRequestDto.Lat,
                Long = addRegionRequestDto.Long,
                Name = addRegionRequestDto.Name,
                Population = addRegionRequestDto.Population
            };

            //Pass Details to Repository
           var response =  await   _regionRepository.AddAsync(region);

            //Convert back to DTO

            var regionDTO = new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionsAsync), new {id = regionDTO.Id}, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync ( Guid id)
        {
            ///Get region from Database
              var region = await _regionRepository.DeleteAsync(id);

            //If nulll Notfound
            if (region == null) {

                return  StatusCode(StatusCodes.Status404NotFound, "invalid id number, please rry again");
                    
              }

            //Convert response back to DTO
            var regionDTO = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            //return ok response to th client
            return StatusCode(StatusCodes.Status200OK,"sucessfully deleted region");

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync( [FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Validate the incoming request
            if (ValidateUpdateRegionAsync(updateRegionRequestDto))
            {
                return BadRequest(ModelState);
            }

            //Convert DTO to Domain

            var region = new Region 
            {
                Code = updateRegionRequestDto.Code,
                Area = updateRegionRequestDto.Area,
                Lat = updateRegionRequestDto.Lat,
                Long = updateRegionRequestDto.Long,
                Name = updateRegionRequestDto.Name,
                Population = updateRegionRequestDto.Population
            };

            //Update Region using Repository

             region = await  _regionRepository.UpdateAsync(id, region);

            //check, if it's Null, then not found
             if (region != null)
            {
                return null;
            }

            //Convert Domain back to DTO

            var regionDTO = new RegionDto
            {
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population

            };

            //Return Ok response

            return StatusCode(StatusCodes.Status200OK, regionDTO);
        }

        #region Private Methods
        private bool  ValidateAddRegionAsync(AddRegionRequestDto addRegionRequestDto)
        {
            //check the mean object is not null, what if the whore request is ematpy

            if(addRegionRequestDto == null)
            {
                ModelState.AddModelError(nameof(addRegionRequestDto.Code),
                    $"Add Region Data is requred.");
                return false;
            }

            //first validate the code in the AddregionRequestDTO
            if(string.IsNullOrWhiteSpace(addRegionRequestDto.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequestDto.Code),
                    $"{nameof(addRegionRequestDto.Code)} cannot be null or empty or white  space.");
            }

            //second Name
            if (string.IsNullOrWhiteSpace(addRegionRequestDto.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequestDto.Name),
                    $"{nameof(addRegionRequestDto.Code)} cannot be null or empty or white  space.");
            }

            //3rd Area
            if (addRegionRequestDto.Area <=0)
            {
                ModelState.AddModelError(nameof(addRegionRequestDto.Area),
                    $"{nameof(addRegionRequestDto.Area)} cannot be less than or equal to zero");
            }

            //4th
            if (addRegionRequestDto.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequestDto.Lat),
                    $"{nameof(addRegionRequestDto.Area)} cannot be less than or equal to zero");
            }
            //5HT
            if (addRegionRequestDto.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequestDto.Long),
                    $"{nameof(addRegionRequestDto.Area)} cannot be less than or equal to zero");
            }
            //6th
            if (addRegionRequestDto.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequestDto.Population),
                    $"{nameof(addRegionRequestDto.Population)} cannot be less than  zero");
            }
            //check it once before  giving the return true
            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateRegionAsync(UpdateRegionRequestDto updateRegionRequestDto)
        {
            //check the mean object is not null, what if the whore request is ematpy

            if (updateRegionRequestDto == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequestDto.Code),
                    $"Add Region Data is requred.");
                return false;
            }

            //first validate the code in the AddregionRequestDTO
            if (string.IsNullOrWhiteSpace(updateRegionRequestDto.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequestDto.Code),
                    $"{nameof(updateRegionRequestDto.Code)} cannot be null or empty or white  space.");
            }

            //second Name
            if (string.IsNullOrWhiteSpace(updateRegionRequestDto.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequestDto.Name),
                    $"{nameof(updateRegionRequestDto.Code)} cannot be null or empty or white  space.");
            }

            //3rd Area
            if (updateRegionRequestDto.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequestDto.Area),
                    $"{nameof(updateRegionRequestDto.Area)} cannot be less than or equal to zero");
            }

            //4th
            if (updateRegionRequestDto.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequestDto.Lat),
                    $"{nameof(updateRegionRequestDto.Area)} cannot be less than or equal to zero");
            }
           
            //check it once before  giving the return true
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }

}
