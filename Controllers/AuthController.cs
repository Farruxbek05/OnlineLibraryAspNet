using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Interfice;
using Swashbuckle.AspNetCore.Annotations;
using TokenRequest = OnlineLibraryAspNet.DTOs.TokenRequest;

namespace OnlineLibraryAspNet.Controllers
{
    public class AuthController:ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthController(IAuthService authService, IJwtTokenService jwtTokenService)
        {
            _authService = authService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Foydalanuvchi tizimga kirishi",
            Description = "Foydalanuvchi tizimga login va parol orqali kirishi."
        )]
        [SwaggerResponse(200, "Muvaffaqiyatli login", typeof(ResponceDto<LoginResponceDto>))]
        [SwaggerResponse(400, "Login yoki parol noto'g'ri", typeof(ResponceDto))]
        [SwaggerResponse(401, "Foydalanuvchi bloklangan", typeof(ResponceDto))]
        public async Task<ActionResult<LoginResponceDto>> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.AuthenticateAsync(request.Username_Or_Email, request.Password);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Foydalanuvchini ro'yxatdan o'tkazish",
            Description = "Yangi foydalanuvchini ro'yxatdan o'tkazish uchun ishlatiladi."
        )]
        [SwaggerResponse(200, "Muvaffaqiyatli ro'yxatdan o'tkazildi", typeof(ResponceDto))]
        [SwaggerResponse(400, "Noto'g'ri so'rov ma'lumotlari", typeof(ResponceDto))]
        [SwaggerResponse(500, "Server ichki xatosi", typeof(ResponceDto))]
        public async Task<ActionResult<LoginResponceDto>> Register([FromBody] UserRegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server ichki xatosi");
            }

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("AccessTokenRefresh")]
        [SwaggerOperation(
        Summary = "Access tokenni yangilash",
            Description = "Amaldagi access token va refresh token orqali yangi access token yaratish."
        )]
        [SwaggerResponse(200, "Token muvaffaqiyatli yangilandi", typeof(ResponceDto<LoginResponceDto>))]
        [SwaggerResponse(400, "Noto'g'ri so'rov ma'lumotlari", typeof(ResponceDto))]
        [SwaggerResponse(401, "Avtorizatsiya talab qilinadi", typeof(ResponceDto))]
        [SwaggerResponse(500, "Server ichki xatosi", typeof(ResponceDto))]
        public async Task<ActionResult<ResponceDto<LoginResponceDto>>> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                // Authorization headerni tekshirish
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return Unauthorized(new ResponceDto<LoginResponceDto>
                    {
                        Success = false,
                        Message = "Authorization header mavjud emas",
                        Data = null
                    });
                }

                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userId = await _authService.GetUserIdFromTokenAsync(token);

                // Yangi tokenni yaratish
                var newToken = await _jwtTokenService.RefreshTokenAsync(token, tokenRequest.RefreshToken);

                return Ok(new ResponceDto<LoginResponceDto>
                {
                    Success = true,
                    Message = "Token muvaffaqiyatli yangilandi",
                    Data = new LoginResponceDto
                    {
                        AccessToken = $"Bearer {newToken.Item1}",
                        RefreshToken = newToken.Item2
                    }
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(new ResponceDto<LoginResponceDto>
                {
                    Success = false,
                    Message = "Access token yoki refresh token noto'g'ri",
                    Data = null
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponceDto<LoginResponceDto>
                {
                    Success = false,
                    Message = "Kutilmagan xato yuz berdi",
                    Data = null
                });
            }
        }

    }
}

