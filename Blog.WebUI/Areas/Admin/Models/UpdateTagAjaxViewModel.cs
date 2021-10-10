using Blog.Entities.DTOs;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class UpdateTagAjaxViewModel
    {
        public string UpdateTagPartial { get; set; }
        public TagDto TagDto { get; set; }
    }
}