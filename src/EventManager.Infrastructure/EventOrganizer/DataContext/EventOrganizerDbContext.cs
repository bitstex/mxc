using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EventManager.Core.EventOrganizer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.Identity.DataContext
{
  public class EventOrganizerDbContext : DbContext
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    public EventOrganizerDbContext(DbContextOptions<EventOrganizerDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
      OnBeforeSaving();
      return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
      OnBeforeSaving();
      return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaving()
    {
      var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
      foreach (var entry in ChangeTracker.Entries<EventEntity>())
      {
        entry.CurrentValues["Owner"] = userId;
        entry.CurrentValues["LastModifiedDate"] = DateTime.Now;
        switch (entry.State)
        {
          case EntityState.Added:
            entry.CurrentValues["IsDeleted"] = false;
            entry.CurrentValues["LastModifiedDate"] = null;
            entry.CurrentValues["CreatedDate"] = DateTime.Now;
            break;

          case EntityState.Deleted:
            entry.State = EntityState.Modified;
            entry.CurrentValues["IsDeleted"] = true;
            break;
        }
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<EventEntity>()
          .Property<bool>("IsDeleted");

      modelBuilder.Entity<EventEntity>()
          .Property<Nullable<DateTime>>("LastModifiedDate");

      modelBuilder.Entity<EventEntity>()
          .Property<string>("Owner")
          .HasMaxLength(20);

      modelBuilder.Entity<EventEntity>()
          .Property(e => e.CreatedDate);

      modelBuilder.Entity<EventEntity>()
          .HasQueryFilter(ev => EF.Property<bool>(ev, "IsDeleted") == false);
    }

    public DbSet<EventEntity> Events { get; set; }
  }
}