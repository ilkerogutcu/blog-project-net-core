using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Tag.Commands
{
    public class CreateTagCommand : IRequest<IResult>
    {
        /// <summary>
        /// Tag name
        /// </summary>
        public string Name { get; set; }
    }
}