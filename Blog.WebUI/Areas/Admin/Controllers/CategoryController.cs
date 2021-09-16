using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.Queries;
using Blog.Core.Extensions;
using Blog.Core.Utilities.Results;
using Blog.WebUI.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAllActiveCategoriesQuery());
            return View("Index", result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            var categories = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveCategories()
        {
            var result = await _mediator.Send(new GetAllActiveCategoriesQuery());
            var categories = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }

        [HttpGet]
        public async Task<JsonResult> GetNotActiveCategories()
        {
            var result = await _mediator.Send(new GetAllNotActiveCategoriesQuery());
            var categories = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCategoryName([FromQuery] string name)
        {
            var result = await _mediator.Send(new GetCategoryByNameQuery
            {
                CategoryName = name
            });
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_AddCategoryPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddCategoryCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    var addCategoryAjaxModel = JsonSerializer.Serialize(new AddCategoryAjaxViewModel
                    {
                        Category = result.Data,
                        AddCategoryPartial = await this.RenderViewToStringAsync("_AddCategoryPartial", command)
                    });
                    return Json(addCategoryAjaxModel);
                }
            }

            var categoryAddAjaxErrorModel = JsonSerializer.Serialize(new AddCategoryAjaxViewModel
            {
                AddCategoryPartial = await this.RenderViewToStringAsync("_AddCategoryPartial", command)
            });

            return Json(categoryAddAjaxErrorModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    var updateCategoryAjaxModel = JsonSerializer.Serialize(new UpdateCategoryAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryUpdatePartial = await this.RenderViewToStringAsync("_UpdateCategoryPartial", command)
                    });
                    return Json(updateCategoryAjaxModel);
                }
            }

            var updateCategoryAjaxErrorModel = JsonSerializer.Serialize(new UpdateCategoryAjaxViewModel
            {
                CategoryUpdatePartial = await this.RenderViewToStringAsync("_UpdateCategoryPartial", command)
            });

            return Json(updateCategoryAjaxErrorModel);
        }

        public async Task<IActionResult> Update(Guid categoryId)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery
            {
                CategoryId = categoryId.ToString()
            });

            if (result.Success)
            {
                return PartialView("_UpdateCategoryPartial", result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            var ajaxResult = JsonSerializer.Serialize(result);
            return Json(ajaxResult);
        }
    }
}