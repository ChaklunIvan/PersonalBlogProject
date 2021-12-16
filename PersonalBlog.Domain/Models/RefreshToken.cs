using PersonalBlog.Domain.Models.Base;

namespace PersonalBlog.Domain.Models
{
    public class RefreshToken : BaseModel
    {
        public string Token { get; set; }
        public User? User { get; set; }
    }
}
