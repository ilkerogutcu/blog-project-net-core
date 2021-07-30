using System;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Tag.Commands
{
    /// <summary>
    ///     Update tag command
    /// </summary>
    public class UpdateTagCommand : IRequest<IResult>
    {
        /// <summary>
        ///     Tag id
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        ///     Tag name
        /// </summary>
        public string Name { get; set; }
    }
}