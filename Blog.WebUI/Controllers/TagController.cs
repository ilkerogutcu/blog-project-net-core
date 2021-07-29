using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Business.Features.Tag.Commands;
using Blog.Business.Features.Tag.Queries;
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
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTagCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteTagCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<IEnumerable<TagDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter paginationFilter)
        {
            var result = await _mediator.Send(new GetAllTagsQuery
            {
                PaginationFilter = paginationFilter,
                Route = Request.Path.Value
            });
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<IEnumerable<TagWithPostsDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("get-with-posts")]
        public async Task<IActionResult> GetByNameWithPosts([FromQuery] PaginationFilter paginationFilter,[FromQuery] string tagName)
        {
            var result = await _mediator.Send(new GetTagByNameWithPostsQuery()
            {
                PaginationFilter = paginationFilter,
                Route = Request.Path.Value,
                Name = tagName
            });
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}