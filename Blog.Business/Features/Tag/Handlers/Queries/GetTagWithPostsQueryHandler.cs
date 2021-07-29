using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Tag.Queries;
using Blog.Business.Helpers;
using Blog.Core.Utilities.Results;
using Blog.Core.Utilities.Uri;
using Blog.DataAccess.Abstract;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Tag.Handlers.Queries
{
    public class GetTagWithPostsQueryHandler : IRequestHandler<GetTagByNameWithPostsQuery, IDataResult<TagWithPostsDto>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public GetTagWithPostsQueryHandler(ITagRepository tagRepository, IMapper mapper, IUriService uriService)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<IDataResult<TagWithPostsDto>> Handle(GetTagByNameWithPostsQuery request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetTagWithPostsAsync(x=>x.Name==request.Name);
            if (tag is null)
            {
                return new ErrorDataResult<TagWithPostsDto>(Messages.DataNotFound);
            }

            var result = _mapper.Map<TagWithPostsDto>(tag);

            return new SuccessDataResult<TagWithPostsDto>(result);
        }
    }
}