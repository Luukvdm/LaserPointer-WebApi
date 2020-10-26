using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Exceptions;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;
using LaserPointer.WebApi.Domain.Events;
using MediatR;

namespace LaserPointer.WebApi.Application.Features.Jobs.Commands.UpdateJobCommand
{
    public class UpdateJobCommand : IRequest
    {
        public int Id { get; set; }
        public JobStatus JobStatus { get; set; }
    }
    
    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateJobCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.FindAsync(request.Id);

            if (job == null)
            {
                throw new NotFoundException(nameof(Job), request.Id);
            }

            job.DomainEvents.Add(new JobStatusChangedEvent(job, request.JobStatus, job.Status));

            job.Status = request.JobStatus;
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
