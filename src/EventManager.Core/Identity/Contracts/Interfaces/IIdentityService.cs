using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EventManager.Core.Identity.Models;

namespace EventManager.Core.Identity.Contracts.Interfaces
{
  public interface IIdentityService
  {
    /// <summary>
    /// Validate the credentials of the user by username and password.
    /// </summary>
    /// <param name="model"></param>
    /// <returns>The list of user claims</returns>
    Task<List<Claim>> LoginAsync(AuthenticationModel model);

    /// <summary>
    /// Registrar a new user with username and password 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task RegistratorAsync(AuthenticationModel model);

    /// <summary>
    /// Create the token based on the user claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns>The signed token and expiration of the token</returns>
    TokenModel GrantTokenForUserClaims(List<Claim> claims);
  }
}