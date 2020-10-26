using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LaserPointer.WebApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetMyJobsQuery
{
    public class GetMyJobsQuery : IRequest<MyJobsVm>
    {
    }
    
    public class GetMyJobsQueryHandler : IRequestHandler<GetMyJobsQuery, MyJobsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetMyJobsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<MyJobsVm> Handle(GetMyJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _context.Jobs
                .Where(j => j.CreatedBy == _currentUser.UserId)
                .ProjectTo<MyJobDto>(_mapper.ConfigurationProvider)
                .OrderBy(j => j.Status)
                .ToListAsync(cancellationToken);
            return new MyJobsVm(jobs);
        }
    }
}
