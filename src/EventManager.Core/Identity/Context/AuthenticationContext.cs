using System.Linq;
using System.Threading.Tasks;
using EventManager.Core.Identity.Contracts.Interfaces;
using EventManager.Core.Identity.Models;

namespace EventManager.Core.Identity.Context
{
  public class AuthenticationContext
  {
    private readonly IIdentityService _identityService;

    public AuthenticationContext(Identity.Contracts.Interfaces.IIdentityService identityService)
    {
      _identityService = identityService;
    }

    public async Task<TokenModel> LoginAsync(AuthenticationModel authUser)
    {
      var claims = await _identityService.LoginAsync(authUser);
      if (!claims?.Any() ?? true)
        return null;
      return _identityService.GrantTokenForUserClaims(claims);
    }

    public async Task Registrator(AuthenticationModel authUser)
    {
      await _identityService.RegistratorAsync(authUser);
    }

  }
}