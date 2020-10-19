using IdentityModel;
using LaserPointer.WebApi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LaserPointer.WebApi.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext?.User != null)
            {
                UserId = httpContextAccessor.HttpContext?.User?.FindFirst(JwtClaimTypes.Subject)?.Value;
                UserName = httpContextAccessor.HttpContext?.User?.FindFirst(JwtClaimTypes.Name)?.Value ?? "";
                UserEmail = httpContextAccessor.HttpContext?.User?.FindFirst(JwtClaimTypes.Email)?.Value ?? "";
                UserRole = httpContextAccessor.HttpContext?.User?.FindFirst(JwtClaimTypes.Role)?.Value ?? "";
            }
        }

        public string UserId { get; }
        public string UserName { get; }
        public string UserEmail { get; }
        public string UserRole { get; }
    }
}
