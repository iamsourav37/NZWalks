using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository.Implementation;
using NZWalks.API.Repository.Interface;
using NZWalks.API.Utility;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenRepository _tokenRepository;
        private ApiResponse _response;
        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ITokenRepository tokenRepository)
        {
            _response = new ApiResponse();
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {

            var identityUser = new IdentityUser()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerDTO.Password);
            if (identityResult.Succeeded)
            {
                if (registerDTO.Roles.Length > 0)
                {
                    for (int i = 0; i < registerDTO.Roles.Length; i++)
                    {
                        var identityRole = await _roleManager.FindByNameAsync(registerDTO.Roles[i]);
                        if (identityRole != null)
                        {
                            await _userManager.AddToRoleAsync(identityUser, registerDTO.Roles[i]);
                        }
                    }

                }
                _response.SetResponse(true, "User Registered Successfully.", null);
                return Ok(_response);

            }
            var errorList = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorList.Add(error.Description);
            }
            _response.SetResponse(false, null, errorList.ToArray());
            return BadRequest(_response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Username);
            if (user != null)
            {
                var isValidPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

                if (isValidPassword)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    // Create the JWT Token
                    var token = _tokenRepository.CreateJWTToken(user, roles.ToList());
                    _response.SetResponse(true, new { token = token, expiresIn = 20 }, null);
                    return Ok(_response);
                }

            }
            _response.SetResponse(false, null, ["Login Failed, Check the username and password"]);
            return BadRequest(_response);
        }
    }
}
