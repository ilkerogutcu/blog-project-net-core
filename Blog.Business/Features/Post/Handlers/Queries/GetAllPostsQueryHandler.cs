using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Post.Queries;
using Blog.Business.Helpers;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.Core.Utilities.Uri;
using Blog.DataAccess.Abstract;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Post.Handlers.Queries
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, IDataResult<IEnumerable<PostDto>>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUriService _uriService;

        public GetAllPostsQueryHandler(IPostRepository postRepository, IUriService uriService)
        {
            _postRepository = postRepository;
            _uriService = uriService;
        }

        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<IEnumerable<PostDto>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var result = (await _postRepository.GetAllWithTags()).ToList();
            return !result.Any()
                ? new ErrorDataResult<List<PostDto>>(Messages.DataNotFound)
                : PaginationHelper.CreatePaginatedResponse(result, request.PaginationFilter, result.Count, _uriService, request.Route);
        }
    }
}