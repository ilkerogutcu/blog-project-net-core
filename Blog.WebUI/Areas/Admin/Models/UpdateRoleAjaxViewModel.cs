using Blog.Entities.DTOs;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class UpdateRoleAjaxViewModel
    {
        public string UpdateRolePartial { get; set; }
        public RoleDto RoleDto { get; set; }
    }
}