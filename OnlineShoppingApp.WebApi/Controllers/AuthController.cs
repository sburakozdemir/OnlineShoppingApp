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
        private readonly IUserService _userService; // Kullanıcı servisinin referansı
        private readonly IConfiguration _configuration; // Yapılandırma ayarları

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService; // Kullanıcı servisinin ataması
            _configuration = configuration; // Yapılandırma ayarlarının ataması
        }

        // Kullanıcı kaydı için endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid) // Modelin geçerli olup olmadığını kontrol et
                return BadRequest(ModelState); // Hatalıysa BadRequest döndür

            var result = await _userService.RegisterUser(new RegisterUserDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate
            });

            // Kullanıcı kaydı sonucuna göre uygun yanıtı döndür
            return result.IsSucceed ? Ok() : BadRequest(result.Message);
        }

        // Kullanıcı girişi için endpoint
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid) // Modelin geçerli olup olmadığını kontrol et
                return BadRequest(ModelState); // Hatalıysa BadRequest döndür

            var result = _userService.LoginUser(new LoginUserDto
            {
                Email = request.Email,
                Password = request.Password
            });

            // Giriş işlemi sonucuna göre uygun yanıtı döndür
            if (!result.IsSucceed)
                return BadRequest(result.Message);

            var user = result.Data; // Kullanıcı bilgilerini al
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

            // Giriş başarılıysa token ile birlikte yanıt döndür
            return Ok(new LoginResponse
            {
                Message = "Login successful",
                Token = token
            });
        }

        // Kullanıcı bilgilerini almak için endpoint
        [HttpGet("me")]
        [Authorize] // Yetkilendirme gerektirir
        public IActionResult GetMyUser()
        {
            return Ok();
        }
    }
}
