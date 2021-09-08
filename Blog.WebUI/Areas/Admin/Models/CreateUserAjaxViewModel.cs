using Blog.Core.Entities.DTOs.Authentication.Requests;
using Blog.Core.Entities.DTOs.Authentication.Responses;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class CreateUserAjaxViewModel
    {
        public string AddUserPartial { get; set; }
        public SignUpResponse SignUpResponse { get; set; }
        public SignUpRequest SignUpRequest { get; set; }
        public string Message { get; set; }
    }
}