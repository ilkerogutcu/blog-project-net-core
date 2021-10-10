using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Features.Tag.Queries;
using Blog.Core.Utilities.Results;
using Blog.Core.Utilities.Uri;
using Blog.DataAccess.Abstract;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Tag.Handlers.Queries
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, IDataResult<TagDto>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public GetTagByIdQueryHandler(ITagRepository tagRepository, IMapper mapper, IUriService uriService)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<TagDto>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetAsync(x => x.Id == request.Id);
            var result = _mapper.Map<TagDto>(tag);
            return new SuccessDataResult<TagDto>(result);
        }
    }
}