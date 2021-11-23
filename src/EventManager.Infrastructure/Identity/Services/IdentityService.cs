using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EventManager.Core.Identity.Contracts.Exceptions;
using EventManager.Core.Identity.Contracts.Interfaces;
using EventManager.Core.Identity.Models;
using EventManager.Infrastructure.Identity.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace EventManager.Infrastructure.Identity.Service
{
  /// <summary>
  /// Create access JWT token for the user
  /// </summary>
  public class IdentityService : IIdentityService
  {
    /// <summary>
    /// Identity manager
    /// </summary>
    private readonly UserManager<ApplicationUser> _userManager;
    /// <summary>
    /// Application configuration
    /// </summary>
    private readonly IConfiguration _configuration;
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Construct identity service
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    public IdentityService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<IdentityService> logger)
    {
      _userManager = userManager;
      _configuration = configuration;
      _logger = logger;
    }

    /// <summary>
    /// Create a new user by username and password
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task RegistratorAsync(AuthenticationModel model)
    {
      _logger.LogTrace(String.Format("RegistratorAsync method called with:{0}", model));
      var userExists = await _userManager.FindByNameAsync(model.Username);
      if (userExists != null)
        throw new UserAlreadyExistException(model);

      ApplicationUser user = new ApplicationUser()
      {
        SecurityStamp = Guid.NewGuid().ToString(),
        UserName = model.Username
      };
      var result = await _userManager.CreateAsync(user, model.Password);
      if (!result.Succeeded)
        throw new CreateUserException(model);

      _logger.LogTrace("RegistratorAsync method ended without error");
    }

    /// <summary>
    /// Check passowrd of the user and create the authentication claims
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<List<Claim>> LoginAsync(AuthenticationModel model)
    {
      _logger.LogTrace(String.Format("LoginAsync method called with:{0}", model));
      var user = await _userManager.FindByNameAsync(model.Username);
      if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
      {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

        foreach (var userRole in userRoles)
        {
          authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        _logger.LogTrace("LoginAsync method ended without error");
        return authClaims;
      }
      _logger.LogWarning(String.Format("User does not exist with name:{0}", model.Username));
      return null;
    }

    /// <summary>
    /// Create JWT access token based on the certain claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public TokenModel GrantTokenForUserClaims(List<Claim> claims)
    {
      _logger.LogTrace(String.Format("GrantTokenForUserClaims method called with:{0}", claims));
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
      var token = new JwtSecurityToken(
          issuer: _configuration["JWT:ValidIssuer"],
          audience: _configuration["JWT:ValidAudience"],
          expires: DateTime.Now.AddHours(3),
          claims: claims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
          );
      _logger.LogTrace("GrantTokenForUserClaims ended without error");
      return new TokenModel
      {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        Expiration = token.ValidTo
      };
    }
  }
}