using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Business.Features.Post.Commands;
using Blog.Business.Features.Post.Queries;
using Blog.Core.Entities.Concrete;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add(AddPostCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePostCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<IEnumerable<PostDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter paginationFilter)
        {
            var result = await _mediator.Send(new GetAllPostsQuery
            {
                PaginationFilter = paginationFilter,
                Route = Request.Path.Value
            });
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<IEnumerable<PostDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("get-all-active")]
        public async Task<IActionResult> GetAllActive([FromQuery] PaginationFilter paginationFilter)
        {
            var result = await _mediator.Send(new GetAllActivePostsQuery
            {
                PaginationFilter = paginationFilter,
                Route = Request.Path.Value
            });
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<IEnumerable<PostDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("get-all-not-active")]
        public async Task<IActionResult> GetAllNotActive([FromQuery] PaginationFilter paginationFilter)
        {
            var result = await _mediator.Send(new GetAllNotActivePostsQuery
            {
                PaginationFilter = paginationFilter,
                Route = Request.Path.Value
            });
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}