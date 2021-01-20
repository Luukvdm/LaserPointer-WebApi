using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Application.Features.HashAlgorithms.Queries.GetHashAlgorithmsQuery
{
    public class GetHashAlgorithmsQuery : IRequest<IList<HashAlgo>>
    {
    }
    
    public class GetHashAlgorithmsQueryHandler : IRequestHandler<GetHashAlgorithmsQuery, IList<HashAlgo>>
    {
        private readonly IApplicationDbContext _context;

        public GetHashAlgorithmsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<HashAlgo>> Handle(GetHashAlgorithmsQuery request, CancellationToken cancellationToken)
        {
            return await _context.HashAlgos.ToListAsync(cancellationToken);
        }
    }
}
