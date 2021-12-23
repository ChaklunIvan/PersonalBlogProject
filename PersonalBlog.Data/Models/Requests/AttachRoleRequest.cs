using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Data.Models.Requests
{
    public class AttachRoleRequest
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string roleName { get; set; }
    }
}
