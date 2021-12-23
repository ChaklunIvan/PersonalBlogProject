using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Roles{ get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Blog>? Blogs {get; set; }
        public virtual ICollection<Comment>? Comments {get; set; }
    }
}
