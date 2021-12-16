using Microsoft.AspNetCore.Identity;

namespace PersonalBlog.Domain.Models
{
    public class Role : IdentityRole<string>
    {
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
