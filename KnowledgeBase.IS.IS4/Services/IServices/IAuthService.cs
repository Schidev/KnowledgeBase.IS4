using KnowledgeBase.IS.IS4.Models.DTOs;

namespace KnowledgeBase.IS.IS4.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}