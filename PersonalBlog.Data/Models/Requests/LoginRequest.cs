
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Data.Requests
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
