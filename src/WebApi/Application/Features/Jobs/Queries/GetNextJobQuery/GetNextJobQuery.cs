using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetNextJobQuery
{
    public class GetNextJobQuery : IRequest<Job>
    {
    }

    public class GetNextJobQueryHandler : IRequestHandler<GetNextJobQuery, Job>
    {
        private readonly IApplicationDbContext _context;

        public GetNextJobQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Job> Handle(GetNextJobQuery request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs
                .Where(j => j.Status == JobStatus.InQueue)
                .OrderBy(j => j.Created)
                .Include(j => j.HashesToCrack)
                .FirstOrDefaultAsync(cancellationToken);

            return job;
        }
    }
}
