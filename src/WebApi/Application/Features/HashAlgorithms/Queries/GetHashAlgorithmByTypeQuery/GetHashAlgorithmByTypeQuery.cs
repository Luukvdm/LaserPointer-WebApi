using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Application.Features.HashAlgorithms.Queries.GetHashAlgorithmByTypeQuery
{
    public class GetHashAlgorithmByTypeQuery : IRequest<HashAlgo>
    {
        public HashAlgoType HashAlgoType { get; set; }
    }
    
    public class GetHashAlgorithmByTypeQueryHandler : IRequestHandler<GetHashAlgorithmByTypeQuery, HashAlgo>
    {
        private readonly IApplicationDbContext _context;

        public GetHashAlgorithmByTypeQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<HashAlgo> Handle(GetHashAlgorithmByTypeQuery request, CancellationToken cancellationToken)
        {
            return _context.HashAlgos.Where(e => e.Type == request.HashAlgoType)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
