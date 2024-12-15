using Microsoft.AspNetCore.Identity;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Enum;
using OnlineLibraryAspNet.Interfice;
using OnlineLibraryAspNet.IRepositorys;
using OnlineLibraryAspNet.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace OnlineLibraryAspNet.Service
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher,
                           IEmailService emailService,
                           IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ResponceDto<LoginResponceDto>> AuthenticateAsync(string username_or_email, string password)
        {
            var user = await _userRepository.FindUsernameOrEmailAsync(username_or_email.ToLower());

            if (!user.Success)
            {
                return new ResponceDto<LoginResponceDto>
                {
                    Success = false,
                    Message = "Username or Passwor is incorrect",
                    StatusCode = 400,
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(new User(), user.Data.HashPassword, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return new ResponceDto<LoginResponceDto>
                {
                    Success = false,
                    Message = "Username or Passwor is incorrect",
                    StatusCode = 400,
                };
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user.Data);

            return new ResponceDto<LoginResponceDto>
            {
                Success = true,
                Data = new LoginResponceDto
                {
                    AccessToken = $"Bearer {token.Item1}",
                    RefreshToken = token.Item2,
                },
                Message = "Successfully login",
                StatusCode = 200,
            };

        }

        public async Task<ResponceDto<string>> GetUserIdFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                return new ResponceDto<string>
                {
                    Success = false,
                    Message = "Invalid token format.",
                    Data = null
                };
            }

            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            var roleClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

            if (userIdClaim == null)
            {
                return new ResponceDto<string>
                {
                    Success = false,
                    Message = "User ID not found in token.",
                    Data = null
                };
            }

            return new ResponceDto<string>
            {
                Success = true,
                Message = "User ID retrieved successfully.",
                Data = userIdClaim.Value
            };
        }

        public async Task<ResponceDto> RegisterAsync(UserRegisterDto registerDto)
        {
            // Username va Email tekshirish
            var existingUser = await _userRepository.FindUsernameOrEmailAsync(registerDto.Username.ToLower(), registerDto.Email.ToLower());

            if (existingUser.Success)
            {
                return new ResponceDto
                {
                    Success = false,
                    Message = "Ushbu Username yoki Email boshqa foydalanuvchiga tegishli.",
                    StatusCode = 400,
                };
            }

          

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                MiddleName = registerDto.MiddleName,
                Username = registerDto.Username.ToLower(),
                Email = registerDto.Email.ToLower(),
                HashPassword = _passwordHasher.HashPassword(new User(), registerDto.Password),
                Role = UserRole.Unknown,
        
                Gender = registerDto.Gender,
            };

            var result = await _userRepository.CreateAsync(user);

            if (!result.Success)
            {
                return new ResponceDto
                {
                    Success = false,
                    Message = "Foydalanuvchi ro'yxatdan o'tkazishda xatolik yuz berdi.",
                    StatusCode = 500,
                };
            }

            await _emailService.SendWelcomeEmailAsync(registerDto.Email, registerDto.FirstName, registerDto.Username, registerDto.Password);

            return new ResponceDto
            {
                Success = true,
                Message = "Foydalanuvchi muvaffaqiyatli ro'yxatdan o'tdi.",
                StatusCode = 200,
            };
        }
    }
}
