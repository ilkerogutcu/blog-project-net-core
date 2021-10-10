using System;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Tag.Queries
{
    public class GetTagByIdQuery : IRequest<IDataResult<TagDto>>
    {
        public Guid Id { get; set; }
    }
}