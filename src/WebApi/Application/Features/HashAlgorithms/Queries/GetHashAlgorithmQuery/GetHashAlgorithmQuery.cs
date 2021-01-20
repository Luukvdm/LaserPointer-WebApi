using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Entities;
using MediatR;

namespace LaserPointer.WebApi.Application.Features.HashAlgorithms.Queries.GetHashAlgorithmQuery
{
    public class GetHashAlgorithmQuery : IRequest<HashAlgo>
    {
        public GetHashAlgorithmQuery(int hashAlgoId)
        {
            Id = hashAlgoId;
        }
        public int Id { get; }
    }
    
    public class GetHashAlgorithmQueryHandler : IRequestHandler<GetHashAlgorithmQuery, HashAlgo>
    {
        private readonly IApplicationDbContext _context;

        public GetHashAlgorithmQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HashAlgo> Handle(GetHashAlgorithmQuery request, CancellationToken cancellationToken)
        {
            return await _context.HashAlgos.FindAsync(new object[] { request.Id }, cancellationToken);
        }
    }
}
