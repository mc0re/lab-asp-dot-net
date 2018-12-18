using Microsoft.AspNetCore.Identity;

namespace SecureApiLab
{
    public class ApiUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
