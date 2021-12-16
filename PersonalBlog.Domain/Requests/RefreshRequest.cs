using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Domain.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
