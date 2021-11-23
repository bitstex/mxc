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
using Microsoft.IdentityModel.Tokens;


namespace EventManager.Infrastructure.Identity.Service
{
  public class IdentityService : IIdentityService
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
      _userManager = userManager;
      _configuration = configuration;
    }

    public async Task RegistratorAsync(AuthenticationModel model)
    {
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

    }

    public async Task<List<Claim>> LoginAsync(AuthenticationModel model)
    {
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
        return authClaims;
      }
      return null;
    }

    public TokenModel GrantTokenForUserClaims(List<Claim> claims)
    {
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
      var token = new JwtSecurityToken(
          issuer: _configuration["JWT:ValidIssuer"],
          audience: _configuration["JWT:ValidAudience"],
          expires: DateTime.Now.AddHours(3),
          claims: claims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
          );

      return new TokenModel
      {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        Expiration = token.ValidTo
      };
    }
  }
}