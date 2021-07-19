using System;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Category.Commands
{
    public class UpdateCategoryCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}