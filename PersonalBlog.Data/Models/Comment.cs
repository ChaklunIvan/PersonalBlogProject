using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Models
{
    public class Comment : BaseModel
    {
        public string Description { get; set; }
        public User? User { get; set; }
        public Article? Article { get; set; }
    }
}
