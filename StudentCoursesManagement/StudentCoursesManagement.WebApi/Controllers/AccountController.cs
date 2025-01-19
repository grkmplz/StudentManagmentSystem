using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.WebApi.Dtos.User;
using StudentCoursesManagement.WebApi.Services;
using StudentCoursesManagement.WebApi.Services.Interfaces;

namespace StudentCoursesManagement.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _registerService;

        public AccountController(IAuthenticationService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegistrationDto userForRegistrationDto)
        {
            var result = await _registerService.RegisterUser(userForRegistrationDto);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return BadRequest(errors);
            }

            return StatusCode(201);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForAuthenticationDto userForAuthenticationDto)
        {
            var checkStatus = await _registerService.ValidateUser(userForAuthenticationDto);

            if (!checkStatus)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Token = await _registerService.CreateToken()
            });
        }

        
    }
}
