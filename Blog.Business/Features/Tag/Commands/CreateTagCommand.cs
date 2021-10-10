using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Tag.Commands
{
    public class CreateTagCommand : IRequest<IDataResult<TagDto>>
    {
        /// <summary>
        /// Tag name
        /// </summary>
        public string Name { get; set; }
    }
}