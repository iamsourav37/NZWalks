using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository.Interface;
using NZWalks.API.Utility;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Reader")]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this._walkRepository = walkRepository;
            this._mapper = mapper;
            this._response = new ApiResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            if (!string.IsNullOrEmpty(filterOn))
            {
                filterOn = filterOn.Trim().ToLower();
                if (string.IsNullOrEmpty(filterQuery))
                {
                    _response.SetResponse(false, null, ["filterQuery is missing", "Please pass the value for filterQuery"]);
                    return BadRequest(_response);
                }

                string[] allowedFilterFields = [nameof(WalkDTO.Name).ToLower(), nameof(WalkDTO.LengthInKm).ToLower()];

                if (!allowedFilterFields.Contains(filterOn))
                {
                    _response.SetResponse(false, null, [$"'{filterOn}' is not a valid filterOn fields", $"Valid fields are '{string.Join(", ", allowedFilterFields)}'"]);
                    return BadRequest(_response);
                }

                var propertyInfo = typeof(WalkDTO).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, filterOn, StringComparison.OrdinalIgnoreCase));

                var dataTypeOfFilterOn = propertyInfo.PropertyType;
                if (dataTypeOfFilterOn == typeof(double))
                {
                    var isValidDataType = double.TryParse(filterQuery, out _);
                    if (!isValidDataType)
                    {
                        _response.SetResponse(false, null, [$"{filterQuery} is not a valid data type for {filterOn}",
                $"Please pass a valid value of type {dataTypeOfFilterOn.Name} for {filterOn}"]);
                        return BadRequest(_response);
                    }
                }


            }
            var walkList = await _walkRepository.GetAll(filterOn, filterQuery);
            var walkDtoList = _mapper.Map<List<WalkDTO>>(walkList);
            _response.SetResponse(true, walkDtoList, null);
            return Ok(_response);
        }

        [HttpGet("{walkId:Guid}")]
        public async Task<IActionResult> Get(Guid walkId)
        {
            var walk = await _walkRepository.GetById(walkId);
            if (walk == null)
            {
                _response.SetResponse(false, null, [$"'{walkId}' not found."]);
                return NotFound(_response);
            }

            var walkDto = _mapper.Map<WalkDTO>(walk);
            _response.SetResponse(true, walkDto, null);
            return Ok(_response);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateWalkDTO createWalkDTO)
        {
            var walk = _mapper.Map<Walk>(createWalkDTO);
            await _walkRepository.Create(walk);
            var walkDto = _mapper.Map<WalkDTO>(walk);
            return CreatedAtAction(nameof(Get), new { walkId = walkDto.Id }, _response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateWalkDTO updateWalkDTO)
        {
            var walk = _mapper.Map<Walk>(updateWalkDTO);
            await _walkRepository.Update(walk);
            return NoContent();
        }


        [HttpDelete("{walkId:guid}")]
        public async Task<IActionResult> Delete(Guid walkId)
        {
            await _walkRepository.Delete(walkId);
            return NoContent();
        }
    }
}
