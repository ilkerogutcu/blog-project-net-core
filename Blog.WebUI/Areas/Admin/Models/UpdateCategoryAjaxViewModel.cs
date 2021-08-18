using Blog.Entities.DTOs;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class UpdateCategoryAjaxViewModel
    {
        public CategoryDto CategoryDto { get; set; }
        public string CategoryUpdatePartial { get; set; }
    }
}