using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.Queries;
using Blog.Core.Extensions;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using Blog.WebUI.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<IEnumerable<CategoryDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                var result = await _mediator.Send(new GetAllCategoriesQuery());
                return View("Index",result);
            }
            
            switch (status)
            {
                case nameof(Status.Active):
                {
                    var result = await _mediator.Send(new GetAllActiveCategoriesQuery());
                    return View("Index",result);
                }
                case nameof(Status.NotActive):
                {
                    var result = await _mediator.Send(new GetAllNotActiveCategoriesQuery());
                    return View("Index",result);
                }
                default:
                    return View("Index",new ErrorDataResult<IEnumerable<CategoryDto>>(Messages.FetchDataFailed));
            }
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
        public  IActionResult Add()
        {
            return PartialView("_AddCategoryPartial");
        }
        
        [HttpPost]
        public async  Task<IActionResult> Add([FromForm] AddCategoryCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    var addCategoryAjaxModel = new AddCategoryAjaxViewModel()
                    {
                        Category = result.Data,
                        AddCategoryPartial = await this.RenderViewToStringAsync("_AddCategoryPartial", command)
                    };
                    return Json(new SuccessDataResult<AddCategoryAjaxViewModel>(addCategoryAjaxModel,result.Message));

                }
            }
            var categoryAddAjaxErrorModel= new AddCategoryAjaxViewModel()
            {
                AddCategoryPartial = await this.RenderViewToStringAsync("_AddCategoryPartial", command)
            };
            return Json(new ErrorDataResult<AddCategoryAjaxViewModel>(categoryAddAjaxErrorModel,Messages.AddFailed));
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IDataResult<CategoryDto>))]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}