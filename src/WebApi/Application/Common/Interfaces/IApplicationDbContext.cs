using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Job> Jobs { get; set; }

        DbSet<Hash> Hashes { get; set; }
        DbSet<HashAlgo> HashAlgos { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
