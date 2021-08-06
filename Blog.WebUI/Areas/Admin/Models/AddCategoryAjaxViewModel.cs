using Blog.Entities.DTOs;

namespace Blog.WebUI.Areas.Admin.Models
{
    public class AddCategoryAjaxViewModel
    {
        public string AddCategoryPartial { get; set; }
        public CategoryDto Category { get; set; }
    }
}