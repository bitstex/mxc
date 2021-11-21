using System.Threading.Tasks;
using EventManager.API.Common.Models;
using EventManager.API.Models;

namespace EventManager.API.Common.Contracts
{
  public interface IIdentityService
  {
    Task<string> GetUserNameAsync(string userId);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

  }
}