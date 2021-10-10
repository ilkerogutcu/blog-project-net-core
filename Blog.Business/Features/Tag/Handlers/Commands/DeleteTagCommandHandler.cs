using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Tag.Commands;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using MediatR;

namespace Blog.Business.Features.Tag.Handlers.Commands
{
    /// <summary>
    ///  Delete tag
    /// </summary>
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, IResult>
    {
        private readonly ITagRepository _tagRepository;

        public DeleteTagCommandHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetTagWithPostsAsync(x => x.Name.Equals(request.Name));
            if (tag is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            if (tag.Posts.Count>0)
            {
                return new ErrorResult(Messages.DataCannotBeDeletedBecauseItHasAnotherData);
            }

            _tagRepository.Delete(tag);
            var result = await _tagRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new SuccessResult(Messages.DataAddedSuccessfully);
            }

            return new ErrorResult(Messages.DeleteFailed);
        }
    }
}