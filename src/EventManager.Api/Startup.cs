using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventOrganizer.Infrastructure.Identity;
using EventOrganizer.Infrastructure.Persistence;
using EventOrganizer.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel.Contracts;
using EventOrganizer.Infrastructure;
using MediatR;
using NSwag.Generation.Processors.Security;
using NSwag;

namespace EventManager.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      Configuration["UseInMemoryDatabase"] = Boolean.TrueString;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMediatR(typeof(Startup));
      services.AddInfrastructure(Configuration);
      services.AddDatabaseDeveloperPageExceptionFilter();
      services.AddOpenApiDocument(options =>
           {
             options.DocumentName = "v1";
             options.Title = "Event Manager API";
             options.Version = "v1";

             options.AddSecurity("oauth2", new OpenApiSecurityScheme
             {
               Type = OpenApiSecuritySchemeType.OAuth2,
               Flows = new OpenApiOAuthFlows
               {
                 AuthorizationCode = new OpenApiOAuthFlow
                 {
                   AuthorizationUrl = "http://localhost:5000/connect/authorize",
                   TokenUrl = "http://localhost:5000/connect/token",
                   Scopes = new Dictionary<string, string> { { "v1", "Event Manager" } }
                 }
               }
             });

             options.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2"));
           });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      if (env.IsDevelopment())
      {
        app.UseOpenApi();
        app.UseSwaggerUi3();
        app.UseDeveloperExceptionPage();
      }
      app.UseRouting();

      app.UseAuthentication();
      app.UseIdentityServer();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
