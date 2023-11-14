using KnowledgeBase.IS.IS4.Data;
using KnowledgeBase.IS.IS4.Models.Auth;
using KnowledgeBase.IS.IS4.Models.DTOs;
using KnowledgeBase.IS.IS4.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace KnowledgeBase.IS.IS4.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AuthDbContext db, UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDTO() { User = null, Token = "" };
            }

            //if user was found , Generate JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDTO userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDTO loginResponseDto = new()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                FullName = registrationRequestDTO.FullName,
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);

                if (!result.Succeeded)
                {
                    return result.Errors.FirstOrDefault().Description;
                }

                var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDTO.Email);

                UserDTO userDto = new()
                {
                    Email = userToReturn.Email,
                    ID = userToReturn.Id,
                    FullName = userToReturn.FullName,
                    PhoneNumber = userToReturn.PhoneNumber
                };

                return "There are no errors occured.";
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered.";
        }
    }
}