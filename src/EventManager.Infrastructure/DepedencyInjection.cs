using EventManager.Core.Identity.Contracts.Interfaces;
using EventManager.Infrastructure.Identity.DataContext;
using EventManager.Infrastructure.Identity.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventManager.Infrastructure
{
  public static class DepedencyInjection
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<IdentitfyDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ConnStr"), b => b.MigrationsAssembly("EventManager.Infrastructure")));
      services.AddDbContext<EventOrganizerDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ConnStr"), b => b.MigrationsAssembly("EventManager.Infrastructure")));

      // For Identity  
      services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<IdentitfyDbContext>()
          .AddDefaultTokenProviders();

      // Adding Authentication  
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })

      // Adding Jwt Bearer  
      .AddJwtBearer(options =>
      {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidAudience = configuration["JWT:ValidAudience"],
          ValidIssuer = configuration["JWT:ValidIssuer"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
      });

      // Service for support to authentication operation in the domain context
      services.AddTransient<IIdentityService, IdentityService>();
      return services;
    }
  }
}