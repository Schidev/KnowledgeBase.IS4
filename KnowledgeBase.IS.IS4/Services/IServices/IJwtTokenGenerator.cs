using KnowledgeBase.IS.IS4.Models.Auth;

namespace KnowledgeBase.IS.IS4.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}