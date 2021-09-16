using Blog.Core.Entities.DTOs.Authentication.Responses;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class UpdateUserAjaxViewModel
    {
        public UserResponse UserResponse { get; set; }
        public string UpdateUserPartial { get; set; }
    }
}