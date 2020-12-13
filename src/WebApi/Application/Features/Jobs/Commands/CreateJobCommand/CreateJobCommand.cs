using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Exceptions;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;
using LaserPointer.WebApi.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Application.Features.Jobs.Commands.CreateJobCommand
{
    public class CreateJobCommand : IRequest<int>
    {
        public HashType HashType { get; set; }
        public IList<string> HexHashes { get; set; }
    }

    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public CreateJobCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            // TODO replace with role?
            if (_currentUser.UserEmail != "admin@laserpointer.com")
            {
                var usersJobs = await _context.Jobs
                    .Where(j => j.CreatedBy == _currentUser.UserId)
                    .Where(j => j.Status != JobStatus.Done)
                    .ToListAsync(cancellationToken);

                if (usersJobs.Count > 1) throw new UserLimitException(typeof(Job).ToString(), _currentUser.UserId);
            }

            var hashes = request.HexHashes.Select(e => new Hash().FromHexString(e)).ToList();
            var job = new Job
            {
                Status = JobStatus.InQueue,
                HashType = request.HashType
            };
            ((List<Hash>) job.HashesToCrack).AddRange(hashes);
            // job.HashesToCrack.ToList().AddRange(hashes);
            job.DomainEvents.Add(new JobCreatedEvent(job));
            
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync(cancellationToken);
            return job.Id;
        }
    }
}
