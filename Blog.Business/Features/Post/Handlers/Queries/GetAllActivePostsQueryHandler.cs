using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Post.Queries;
using Blog.Business.Helpers;
using Blog.Core.Utilities.Results;
using Blog.Core.Utilities.Uri;
using Blog.DataAccess.Abstract;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Post.Handlers.Queries
{
    public class GetAllActivePostsQueryHandler : IRequestHandler<GetAllActivePostsQuery, IDataResult<IEnumerable<PostDto>>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUriService _uriService;
        
        public GetAllActivePostsQueryHandler(IPostRepository postRepository, IUriService uriService)
        {
            _postRepository = postRepository;
            _uriService = uriService;
        }
        public async Task<IDataResult<IEnumerable<PostDto>>> Handle(GetAllActivePostsQuery request, CancellationToken cancellationToken)
        {
            var result = (await _postRepository.GetAllByStatusWithTags(true)).ToList();
            return !result.Any()
                ? new ErrorDataResult<List<PostDto>>(Messages.DataNotFound)
                : PaginationHelper.CreatePaginatedResponse(result, request.PaginationFilter, result.Count, _uriService, request.Route);
        }
    }
}