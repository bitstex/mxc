using EventOrganizer.Infrastructure.Identity;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedKernel.Contracts;
using SharedKernel.Entities;
using SharedKernel.Events;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EventOrganizer.Infrastructure.Persistence
{
  public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
  {
    private readonly ICurrentUserService _currentUserService;
    private readonly IDomainEventService _domainEventService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        ICurrentUserService currentUserService,
        IDomainEventService domainEventService) : base(options, operationalStoreOptions)
    {
      _currentUserService = currentUserService;
      _domainEventService = domainEventService;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      var events = ChangeTracker.Entries<BaseEntity<Guid>>()
              .Select(x => x.Entity.Events)
              .SelectMany(x => x)
              .ToArray();

      var result = await base.SaveChangesAsync(cancellationToken);

      await DispatchEvents(events);

      return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      base.OnModelCreating(builder);
    }

    private async Task DispatchEvents(BaseDomainEvent[] events)
    {
      foreach (var @event in events)
      {
        await _domainEventService.Publish(@event);
      }
    }
  }
}
