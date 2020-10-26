using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Exceptions;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Entities;
using MediatR;

namespace LaserPointer.WebApi.Application.Features.Hashes.Commands.UpdateHashCommand
{
    public class UpdateHashCommand : IRequest
    {
        public int Id { get; set; }
        public string PlainValue { get; set; }
    }
    
    public class UpdateHashCommandHandler : IRequestHandler<UpdateHashCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateHashCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateHashCommand request, CancellationToken cancellationToken)
        {
            var hash = await _context.Hashes.FindAsync(request.Id);

            if (hash == null)
            {
                throw new NotFoundException(nameof(Hash), request.Id);
            }

            hash.PlainValue = request.PlainValue;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
