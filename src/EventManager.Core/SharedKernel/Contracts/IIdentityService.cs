using System.Threading.Tasks;
using SharedKernel.Models;

namespace SharedKernel.Contracts
{
  public interface IIdentityService
  {
    Task<string> GetUserNameAsync(string userId);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

  }
}