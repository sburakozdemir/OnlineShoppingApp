using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Services;
using OnlineShoppingApp.WebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using OnlineShoppingApp.Business.Operations.User.Dtos;
using OnlineShoppingApp.Business.Operations.User;
using OnlineShoppingApp.WebApi.Jwt;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUser(new RegisterUserDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate
            });

            return result.IsSucceed ? Ok() : BadRequest(result.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _userService.LoginUser(new LoginUserDto
            {
                Email = request.Email,
                Password = request.Password
            });

            if (!result.IsSucceed)
                return BadRequest(result.Message);

            var user = result.Data;
            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                SecretKey = _configuration["Jwt:SecretKey"],
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                ExpireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"])
            });

            return Ok(new LoginResponse
            {
                Message = "Login successful",
                Token = token
            });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMyUser()
        {
            return Ok(); // Kullanıcı bilgilerini döndürebilirsin
        }
    }
}
