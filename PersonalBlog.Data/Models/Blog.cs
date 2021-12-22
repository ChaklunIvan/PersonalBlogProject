using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Models
{
    public class Blog : BaseModel
    {
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Article>? Articles { get; set; }

    }
}
