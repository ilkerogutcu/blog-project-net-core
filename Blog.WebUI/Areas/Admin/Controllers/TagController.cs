using System;
using System.Text.Json;
using System.Threading.Tasks;
using Blog.Business.Features.Tag.Commands;
using Blog.Business.Features.Tag.Queries;
using Blog.Core.Extensions;
using Blog.WebUI.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TagController : Controller
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAllTagsQuery());
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateTagPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    var createTagAjaxModel = JsonSerializer.Serialize(new CreateTagAjaxViewModel
                    {
                        CreateTagPartial = await this.RenderViewToStringAsync("_CreateTagPartial", command),
                        TagDto = result.Data,
                        Message = result.Message
                    });
                    return Json(createTagAjaxModel);
                }
            }

            var createTagAjaxErrorModel = JsonSerializer.Serialize(new CreateTagAjaxViewModel
            {
                CreateTagPartial = await this.RenderViewToStringAsync("_CreateTagPartial", command),
                TagName = command.Name
            });
            return Json(createTagAjaxErrorModel);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var tagDto = await _mediator.Send(new GetTagByIdQuery
            {
                Id = id
            });

            return PartialView("_UpdateTagPartial", new UpdateTagCommand
            {
                Name = tagDto.Data.Name,
                Id = tagDto.Data.Id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTagCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    var updateTagAjaxModel = JsonSerializer.Serialize(new UpdateTagAjaxViewModel
                    {
                        TagDto = result.Data,
                        UpdateTagPartial = await this.RenderViewToStringAsync("_UpdateTagPartial", command)
                    });
                    return Json(updateTagAjaxModel);
                }
            }

            var updateTagErrorAjaxModel = JsonSerializer.Serialize(new UpdateTagAjaxViewModel
            {
                UpdateTagPartial = await this.RenderViewToStringAsync("_UpdateTagPartial", command)
            });

            return Json(updateTagErrorAjaxModel);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteTagCommand command)
        {
            var result = await _mediator.Send(command);
            var ajaxResult = JsonSerializer.Serialize(result);
            return Json(ajaxResult);
        }
    }
}