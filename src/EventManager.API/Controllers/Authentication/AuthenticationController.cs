using System;
using System.Threading.Tasks;
using EventManager.Core.Identity.Context;
using EventManager.Core.Identity.Contracts.Interfaces;
using EventManager.Core.Identity.Models;
using EventManager.Infrastructure.Identity.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EventManager.API.Controllers.Authentication
{
  /// <summary>
  /// Authentication controller to login or register a user
  /// </summary>
  [Route("api/v1/auth")]
  [ApiController]
  public class AuthenticateController : ControllerBase
  {
    /// <summary>
    /// User manager from the Identity package
    /// </summary>
    private readonly UserManager<ApplicationUser> userManager;
    /// <summary>
    /// Microsoft extension to handling the appsettings
    /// </summary>
    private readonly IConfiguration _configuration;
    /// <summary>
    /// Domain context to login or register a user
    /// </summary>
    private AuthenticationContext _authenticationContext;

    /// <summary>
    /// Construct the authentication controller
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="configuration"></param>
    /// <param name="identityService"></param>
    public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IIdentityService identityService)
    {
      this.userManager = userManager;
      _configuration = configuration;
      _authenticationContext = new AuthenticationContext(identityService);
    }

    /// <summary>
    /// Generate the access token by username and password of the user
    /// </summary>
    /// <param name="model">Username and password of the user</param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticationModel model)
    {
      var token = await _authenticationContext.LoginAsync(model);
      return String.IsNullOrEmpty(token?.Token) ? Unauthorized() : Ok(token);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="model">Credentials of the user</param>
    /// <returns></returns>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] AuthenticationModel model)
    {
      await _authenticationContext.Registrator(model);
      return Ok();
    }
  }
}