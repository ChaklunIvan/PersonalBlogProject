using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Models
{
    public class Tag : BaseModel
    {
        public string Value { get; set; }
        public virtual ICollection<Article>? Articles { get; set; }  
    }
}
