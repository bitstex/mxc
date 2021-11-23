using System;
using System.Threading.Tasks;
using EventManager.Core.Identity.Context;
using EventManager.Core.Identity.Contracts.Interfaces;
using EventManager.Core.Identity.Models;
using EventManager.Infrastructure.Identity.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
    /// Microsoft extension to handling the appsettings
    /// </summary>
    private readonly IConfiguration _configuration;
    /// <summary>
    /// Domain context to login or register a user
    /// </summary>
    private AuthenticationContext _authenticationContext;
    /// <summary>
    /// Logger instacne constructed by DI
    /// </summary>
    private ILogger<AuthenticateController> _logger;

    /// <summary>
    /// Construct the authentication controller
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="configuration"></param>
    /// <param name="identityService"></param>
    public AuthenticateController(IConfiguration configuration, IIdentityService identityService, ILogger<AuthenticateController> logger)
    {
      _configuration = configuration;
      _authenticationContext = new AuthenticationContext(identityService);
      _logger = logger;
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
      _logger.LogTrace(String.Format("HTTP Post request is received with: {0}", model));
      var token = await _authenticationContext.LoginAsync(model);
      _logger.LogDebug(String.Format("token: {0}", token));
      if (String.IsNullOrEmpty(token?.Token))
        return Unauthorized();

      _logger.LogInformation("Token created for the user");
      return Ok(token);
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
      _logger.LogTrace(String.Format("HTTP Post request is received with: {0}", model));
      await _authenticationContext.Registrator(model);
      _logger.LogInformation("The user created with credentials");
      return Ok();
    }
  }
}