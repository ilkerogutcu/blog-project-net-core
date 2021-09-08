using System;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
    public class DeleteUserCommand: IRequest<IResult>
    {
        public string Id { get; set; }
    }
}