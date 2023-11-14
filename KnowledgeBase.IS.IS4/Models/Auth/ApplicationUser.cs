using Microsoft.AspNetCore.Identity;

namespace KnowledgeBase.IS.IS4.Models.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}