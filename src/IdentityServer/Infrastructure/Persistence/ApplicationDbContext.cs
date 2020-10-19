using IdentityServer4.EntityFramework.Options;
using LaserPointer.IdentityServer.Common.Interfaces;
using LaserPointer.IdentityServer.Infrastructure.Identity;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LaserPointer.IdentityServer.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}
