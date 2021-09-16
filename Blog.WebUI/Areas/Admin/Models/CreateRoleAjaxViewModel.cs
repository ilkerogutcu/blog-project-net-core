using Blog.Entities.DTOs;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class CreateRoleAjaxViewModel
    {
        public string CreateRolePartial { get; set; }
        public string Role { get; set; }
        public RoleDto RoleDto { get; set; }
        public string Message { get; set; }
    }
}