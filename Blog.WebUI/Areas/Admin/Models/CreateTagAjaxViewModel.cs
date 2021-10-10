using Blog.Entities.DTOs;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class CreateTagAjaxViewModel
    {
        public string CreateTagPartial { get; set; }
        public string TagName { get; set; }
        public TagDto TagDto { get; set; }
        public string Message { get; set; }
    }
}