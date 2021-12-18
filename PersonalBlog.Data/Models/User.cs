using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role? Role { get; set; }
        public string? RoleName { get; set; }
    }
}
