using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LaserPointer.WebApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetJobDetailsQuery
{
    public class GetJobDetailsQuery : IRequest<JobDetailsDto>
    {
        public GetJobDetailsQuery(int id) => Id = id;
        public int Id { get; set; }
    }

    public class GetJobDetailsQueryHandler : IRequestHandler<GetJobDetailsQuery, JobDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        

        public GetJobDetailsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public Task<JobDetailsDto> Handle(GetJobDetailsQuery request, CancellationToken cancellationToken)
        {
            var job = _context.Jobs
                .Include(j => j.HashesToCrack)
                .Where(j => j.Id == request.Id && j.CreatedBy == _currentUser.UserId)
                .ProjectTo<JobDetailsDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            return job;
        }
    }
}
