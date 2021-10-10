using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.Queries;
using Blog.Core.Entities.DTOs.Authentication.Requests;
using Blog.Core.Extensions;
using Blog.WebUI.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAllUsersByStatusQuery
            {
                Status = true
            });
            return View(result);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInCommand signInCommand)
        {
            if (!ModelState.IsValid)
            {
                return View("UserLogin");
            }

            var result = await _mediator.Send(signInCommand);
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", $"{result.Message}");
            return View("UserLogin");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(GetAllUsersQuery query)
        {
            var result = await _mediator.Send(query);
            var users = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(users);
        }

        // [HttpGet("{id:guid}")]
        // public async Task<IActionResult> GetUserById(Guid id)
        // {
        //     var result = await _mediator.Send(new GetUserByIdQuery
        //     {
        //         Id = id
        //     });
        //     return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        // }

        // [HttpGet("{username}")]
        // public async Task<IActionResult> GetUserByUsername(string username)
        // {
        //     var result = await _mediator.Send(new GetUserByUsernameQuery
        //     {
        //         Username = username
        //     });
        //     return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        // }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SignUpAdmin()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());
            ViewBag.Roles = roles.Data.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });
            return PartialView("_AddUserPartial");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignUpAdmin(SignUpRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new SignUpCommand
                {
                    SignUpRequest = request
                });
                if (result.Success)
                {
                    var createUserAjaxModel = JsonSerializer.Serialize(new CreateUserAjaxViewModel
                    {
                        SignUpResponse = result.Data,
                        AddUserPartial = await this.RenderViewToStringAsync("_AddUserPartial", request),
                        SignUpRequest = request,
                        Message = result.Message
                    });
                    return Json(createUserAjaxModel);
                }
            }

            var createUserAjaxErrorModel = JsonSerializer.Serialize(new CreateUserAjaxViewModel
            {
                SignUpRequest = request,
                AddUserPartial = await this.RenderViewToStringAsync("_AddUserPartial", request)
            });
            return Json(createUserAjaxErrorModel);
        }

        [HttpPost("sign-in/2FA")]
        public async Task<IActionResult> SignInWithTwoFactorSecurity(SignInWithTwoFactorQuery query)
        {
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPost("send-email-verification-token")]
        public async Task<IActionResult> SendEmailConfirmationToken(SendEmailConfirmationTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("update-2FA")]
        [Authorize]
        public async Task<IActionResult> UpdateTwoFactorSecurity(UpdateTwoFactorSecurityCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteUserCommand
            {
                Id = id
            });
            var ajaxResult = JsonSerializer.Serialize(result);
            return Json(ajaxResult);
        }

        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery
            {
                Id = id
            });

            if (result.Success)
            {
                return PartialView("_UpdateUserPartial", new UpdateUserCommand
                {
                    Id = id,
                    FirstName = result.Data.FirstName,
                    ImageUrl = result.Data.ImageUrl,
                    LastName = result.Data.LastName,
                    Bio = result.Data.Bio
                });
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(UpdateUserCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    var updateUserAjaxModel = JsonSerializer.Serialize(new UpdateUserAjaxViewModel
                    {
                        UserResponse = result.Data,
                        UpdateUserPartial = await this.RenderViewToStringAsync("_UpdateUserPartial", command)
                    });
                    return Json(updateUserAjaxModel);
                }
            }

            var updateUserAjaxErrorModel = JsonSerializer.Serialize(new UpdateUserAjaxViewModel
            {
                UpdateUserPartial = await this.RenderViewToStringAsync("_UpdateUserPartial", command)
            });

            return Json(updateUserAjaxErrorModel);
        }
    }
}