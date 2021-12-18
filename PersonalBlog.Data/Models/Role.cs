using PersonalBlog.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Data.Models
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public IEnumerable<User>? Users { get; set; }
    }
}
