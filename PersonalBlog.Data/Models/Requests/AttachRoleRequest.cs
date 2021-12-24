using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Data.Models.Requests
{
    public class AttachRoleRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
