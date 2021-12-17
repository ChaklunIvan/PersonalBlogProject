using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Data.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
