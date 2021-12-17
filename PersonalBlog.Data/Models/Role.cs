using Microsoft.AspNetCore.Identity;

namespace PersonalBlog.Data.Models
{
    public class Role : IdentityRole<string>
    {
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
