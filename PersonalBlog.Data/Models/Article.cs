using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Models
{
    public class Article : BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string? BlogTitle { get; set; }
        public virtual Blog? Blog { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
