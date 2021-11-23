using System.Linq;
using System.Threading.Tasks;
using EventManager.Core.Identity.Contracts.Interfaces;
using EventManager.Core.Identity.Models;

namespace EventManager.Core.Identity.Context
{
  /// <summary>
  /// Login and registrator operation handlers
  /// </summary>
  public class AuthenticationContext
  {
    /// <summary>
    /// Interfcae to call IdentityService from the infrastructure layer
    /// </summary>
    private readonly IIdentityService _identityService;
    /// <summary>
    /// Contruct the authentication context
    /// </summary>
    /// <param name="identityService"></param>
    public AuthenticationContext(Identity.Contracts.Interfaces.IIdentityService identityService)
    {
      _identityService = identityService;
    }

    /// <summary>
    /// Create access token based on claims of the user
    /// </summary>
    /// <param name="authUser"></param>
    /// <returns></returns>
    public async Task<TokenModel> LoginAsync(AuthenticationModel authUser)
    {
      var claims = await _identityService.LoginAsync(authUser);
      if (!claims?.Any() ?? true)
        return null;
      return _identityService.GrantTokenForUserClaims(claims);
    }
    /// <summary>
    /// Persist a new user with credentials
    /// </summary>
    /// <param name="authUser"></param>
    /// <returns></returns>
    public async Task Registrator(AuthenticationModel authUser)
    {
      await _identityService.RegistratorAsync(authUser);
    }

  }
}