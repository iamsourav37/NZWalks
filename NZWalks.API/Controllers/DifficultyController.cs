using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repository.Interface;
using NZWalks.API.Utility;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Reader, Writer")]
    public class DifficultyController : ControllerBase
    {
        private readonly IDifficultyRepository _difficultyRepository;
        private ApiResponse _response;

        public DifficultyController(IDifficultyRepository difficultyRepository)
        {
            this._difficultyRepository = difficultyRepository;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var difficultyList = await _difficultyRepository.GetAll();
            _response.SetResponse(true, difficultyList, null);
            return Ok(_response);
        }

        [HttpGet("{difficultyId:Guid}")]
        public async Task<IActionResult> Get(Guid difficultyId)
        {
            var difficulty = await _difficultyRepository.GetById(difficultyId);
            if (difficulty == null)
            {
                _response.SetResponse(false, null, [$"{difficultyId} not found"]);
                return NotFound(_response);
            }

            _response.SetResponse(true, difficulty, null);
            return Ok(_response);
        }
    }
}
