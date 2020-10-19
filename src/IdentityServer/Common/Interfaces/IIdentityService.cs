using System.Threading.Tasks;
using LaserPointer.IdentityServer.Common.Models;

namespace LaserPointer.IdentityServer.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);
    }
}
