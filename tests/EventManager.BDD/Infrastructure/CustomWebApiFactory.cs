using System.Linq;
using EventManager.Infrastructure.Identity.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace webapi.tests.Infrastructure
{
  public class CustomWebApiFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
  {
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.ConfigureServices(services =>
      {
        var descriptor = services.SingleOrDefault(
                  d => d.ServiceType ==
                      typeof(DbContextOptions<IdentifyDbContext>));

        services.Remove(descriptor);

        descriptor = services.SingleOrDefault(
                  d => d.ServiceType ==
                      typeof(DbContextOptions<EventOrganizerDbContext>));

        services.Remove(descriptor);

        services.AddDbContext<IdentifyDbContext>(options => options.UseInMemoryDatabase("IdentityDatabaseIntegrationTest"));
        services.AddDbContext<EventOrganizerDbContext>(options => options.UseInMemoryDatabase("EventOrganizerDatabaseIntegrationTest"));


        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbIdentity = scopedServices.GetRequiredService<IdentifyDbContext>();
        var dbEventOrganizer = scopedServices.GetRequiredService<IdentifyDbContext>();

        var logger = scopedServices
                  .GetRequiredService<ILogger<CustomWebApiFactory<TStartup>>>();

        dbIdentity.Database.EnsureCreated();
        dbEventOrganizer.Database.EnsureCreated();
      });
    }
  }
}