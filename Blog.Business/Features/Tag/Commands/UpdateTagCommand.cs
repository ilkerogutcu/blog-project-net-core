using System;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Tag.Commands
{
    /// <summary>
    ///     Update tag command
    /// </summary>
    public class UpdateTagCommand : IRequest<IDataResult<TagDto>>
    {
        /// <summary>
        ///     Tag ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     New Tag name
        /// </summary>
        public string Name { get; set; }
    }
}