using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Models
{
    public class Comment : BaseModel
    {
        public string Value { get; set; }
        public virtual User? User { get; set; }
        public virtual Article Article { get; set; }
    }
}
