using System.Collections.Generic;
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
    public class GetUnfinishedJobsQuery : IRequest<IList<JobDto>>
    {
    }
    
    public class GetJobsQueryHandler : IRequestHandler<GetUnfinishedJobsQuery, IList<JobDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<JobDto>> Handle(GetUnfinishedJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _context.Jobs
                .ProjectTo<JobDto>(_mapper.ConfigurationProvider)
                .Where(j => j.Status != JobStatus.Done).ToListAsync(cancellationToken);
            return jobs;
        }
    }
}
