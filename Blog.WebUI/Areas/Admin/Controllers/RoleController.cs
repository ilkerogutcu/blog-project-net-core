using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.Queries;
using Blog.Core.Extensions;
using Blog.WebUI.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAllRolesQuery());
            return View(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllRolesQuery());
            var roles = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(roles);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return PartialView("_CreateRolePartial");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateRoleCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    var createRoleAjaxModel = JsonSerializer.Serialize(new CreateRoleAjaxViewModel
                    {
                        CreateRolePartial = await this.RenderViewToStringAsync("_CreateRolePartial", command),
                        RoleDto = result.Data,
                        Message = result.Message
                    });
                    return Json(createRoleAjaxModel);
                }
            }

            var createRoleAjaxErrorModel = JsonSerializer.Serialize(new CreateRoleAjaxViewModel
            {
                CreateRolePartial = await this.RenderViewToStringAsync("_CreateRolePartial", command),
                Role = command.Name
            });
            return Json(createRoleAjaxErrorModel);
        }

        [Authorize]
        public async Task<IActionResult> Update(string name)
        {
            var roleDto = await _mediator.Send(new GetRoleByNameQuery
            {
                Name = name
            });
            return PartialView("_UpdateRolePartial", new UpdateRoleCommand
            {
                Name = roleDto.Data.Name,
                Id = roleDto.Data.Id
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(UpdateRoleCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    var updateRoleAjaxModel = JsonSerializer.Serialize(new UpdateRoleAjaxViewModel
                    {
                        RoleDto = result.Data,
                        UpdateRolePartial = await this.RenderViewToStringAsync("_UpdateRolePartial", command)
                    });
                    return Json(updateRoleAjaxModel);
                }
            }

            var updateRoleErrorAjaxModel = JsonSerializer.Serialize(new UpdateRoleAjaxViewModel
            {
                UpdateRolePartial = await this.RenderViewToStringAsync("_UpdateRolePartial", command)
            });

            return Json(updateRoleErrorAjaxModel);
        }
    }
}