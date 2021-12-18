using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Data.Models.Requests
{
    public class RoleRequest
    {
        [Required]
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
