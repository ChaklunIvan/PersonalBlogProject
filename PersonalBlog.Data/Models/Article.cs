using PersonalBlog.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalBlog.Data.Models
{
    public class Article : BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string? BlogTitle { get; set; }
        [NotMapped]
        public IEnumerable<string>? Tags { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
        public virtual Blog? Blog { get; set; }
    }
}
