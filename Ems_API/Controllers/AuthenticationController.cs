using Ems_Services.Interfaces;
using Ems_Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ems_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IAuthenticationService authService) : ControllerBase
    {
        [HttpPost("registerUser")]
        public async Task<IActionResult> CreateUserAsync(Register userModel)
        {
            if (userModel == null)
            {
                return BadRequest("Model is empty");
            }
            var result = await authService.CreateAccountAsync(userModel);
            return Ok(result);  
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> SignInNAsync(Login loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest("Model is empty");
            }
            var result = await authService.LoginAccountAsync(loginModel);
            return Ok(result);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshToken tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Model is empty");
            }
            var result = await authService.RefreshTokenAsync(tokenModel);
            return Ok(result);
        }

    }
}
