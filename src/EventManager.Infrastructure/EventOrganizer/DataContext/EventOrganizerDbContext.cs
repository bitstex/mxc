using System.Threading;
using System.Threading.Tasks;
using EventManager.Core.EventOrganizer.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.Identity.DataContext
{
  public class EventOrganizerDbContext : DbContext
  {
    public EventOrganizerDbContext(DbContextOptions<EventOrganizerDbContext> options) : base(options)
    {
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
      foreach (var entry in ChangeTracker.Entries<EventEntity>())
      {
        switch (entry.State)
        {
          case EntityState.Added:
            entry.CurrentValues["IsDeleted"] = false;
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
          .HasQueryFilter(ev => EF.Property<bool>(ev, "IsDeleted") == false);
    }

    public DbSet<EventEntity> Events { get; set; }
  }
}