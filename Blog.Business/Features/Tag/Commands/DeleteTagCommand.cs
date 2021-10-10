using System;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Tag.Commands
{
    public class DeleteTagCommand : IRequest<IResult>
    {
        public string Name { get; set; }
    }
}