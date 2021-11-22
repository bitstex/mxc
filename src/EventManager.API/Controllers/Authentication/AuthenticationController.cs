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
  [Route("api/[controller]")]
  [ApiController]
  public class AuthenticateController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IConfiguration _configuration;
    private AuthenticationContext _authenticationContext;

    public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IIdentityService identityService)
    {
      this.userManager = userManager;
      _configuration = configuration;
      _authenticationContext = new AuthenticationContext(identityService);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticationModel model)
    {
      var token = await _authenticationContext.LoginAsync(model);
      return token == null || String.IsNullOrEmpty(token.Token) ? Unauthorized() : Ok(token);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] AuthenticationModel model)
    {
      await _authenticationContext.Registrator(model);
      return Ok(200);
    }
  }
}