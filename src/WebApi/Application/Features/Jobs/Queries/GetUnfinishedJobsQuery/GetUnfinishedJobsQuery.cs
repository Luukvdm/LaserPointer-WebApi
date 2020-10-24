using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetUnfinishedJobsQuery
{
    public class GetUnfinishedJobsQuery : IRequest<UnfinishedJobsVm>
    {
    }
    
    public class GetJobsQueryHandler : IRequestHandler<GetUnfinishedJobsQuery, UnfinishedJobsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UnfinishedJobsVm> Handle(GetUnfinishedJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _context.Jobs
                .ProjectTo<JobDto>(_mapper.ConfigurationProvider)
                .Where(j => j.Status != JobStatus.Done).ToListAsync(cancellationToken);
            return new UnfinishedJobsVm(jobs);
        }
    }
}
