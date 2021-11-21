using System.Collections.Generic;
using System.Security.Claims;
using EventOrganizer.Infrastructure.Identity;
using EventOrganizer.Infrastructure.Persistence;
using EventOrganizer.Infrastructure.Services;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Contracts;

namespace EventOrganizer.Infrastructure
{

  public static class DependencyInjection
  {
    public static IEnumerable<Client> Clients =>
    new List<Client>
    {
        new Client
        {
            ClientId = "client",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "v1" }
        }
    };
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddIdentityServer(options =>
                      {
                        options.Events.RaiseErrorEvents = true;
                        options.Events.RaiseInformationEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseSuccessEvents = true;
                      })
                      .AddTestUsers(new List<TestUser>
                            {
                                new TestUser
                                {
                                    SubjectId = "818727",
                                    Username = "alice",
                                    Password = "alice",
                                }})
                      .AddInMemoryIdentityResources(Config.IdentityResources)
                      .AddInMemoryApiScopes(Config.ApiScopes)
                      .AddInMemoryApiResources(Config.ApiResources)
                      .AddInMemoryClients(Config.Clients)
                      .AddDeveloperSigningCredential();

      services.AddAuthentication(options =>
                {
                  options.DefaultAuthenticateScheme =
                                            JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme =
                                            JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(o =>
                {
                  o.Authority = "http://localhost:5000";
                  o.Audience = "v1";
                  o.RequireHttpsMetadata = false;
                });
      if (configuration.GetValue<bool>("UseInMemoryDatabase"))
      {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("CleanArchitectureDb"));
      }
      else
      {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
      }
      services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

      services.AddScoped<IDomainEventService, DomainEventService>();
      services.AddScoped<ICurrentUserService, CurrentUserService>();

      services
          .AddDefaultIdentity<ApplicationUser>()
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>();


      services.AddTransient<IIdentityService, IdentityService>();

      services.AddAuthentication()
          .AddIdentityServerJwt();

      services.AddAuthorization();

      return services;
    }
  }
}