using System;
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
    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, IDataResult<IEnumerable<TagDto>>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public GetAllTagsQueryHandler(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<IEnumerable<TagDto>>> Handle(GetAllTagsQuery request,
            CancellationToken cancellationToken)
        {
            var tags = await _tagRepository.GetAllAsync();
            var result = _mapper.Map<List<TagDto>>(tags);
            return new SuccessDataResult<IEnumerable<TagDto>>(result);
        }
    }
}