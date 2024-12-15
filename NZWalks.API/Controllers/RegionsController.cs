using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository.Interface;
using NZWalks.API.Utility;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Writer")]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _response = new();
            this._regionRepository = regionRepository;
            this._mapper = mapper;
        }


        // Get All Region
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {

            var regionsList = await _regionRepository.GetAll();
            var regionDtoList = _mapper.Map<List<RegionDTO>>(regionsList);

            _response.SetResponse(true, regionDtoList, null);
            return Ok(_response);
        }


        // Get Single Region by Id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var existingRegion = await _regionRepository.GetById(id);
            if (existingRegion != null)
            {
                var regionDto = _mapper.Map<RegionDTO>(existingRegion);

                _response.SetResponse(true, regionDto, null);
                return Ok(_response);
            }

            _response.SetResponse(false, null, ["id not found", $"{id} not found in the db"]);
            return NotFound(_response);
        }


        // Create Region
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegionCreateDTO regionCreateDto)
        {
            var region = _mapper.Map<Region>(regionCreateDto);
            await _regionRepository.CreateRegion(region);
            var createdRegion = _mapper.Map<RegionDTO>(region);
            _response.SetResponse(true, createdRegion, null);
            return CreatedAtAction(nameof(Get), new { id = region.Id }, _response);

        }


        // Update Region
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RegionUpdateDTO regionUpdateDTO)
        {
            var region = _mapper.Map<Region>(regionUpdateDTO);
            await _regionRepository.UpdateRegion(region);
            var regionDto = _mapper.Map<RegionDTO>(region);
            _response.SetResponse(true, regionDto, null);
            return Ok(_response);
        }


        // Delete Region
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid regioinId)
        {
            await _regionRepository.DeleteRegion(regioinId);
            return NoContent();
        }
    }
}
