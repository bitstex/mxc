using EventManager.Core.EventOrganizer.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.Identity.DataContext
{
  public class EventOrganizerDbContext : DbContext
  {
    public EventOrganizerDbContext(DbContextOptions<EventOrganizerDbContext> options) : base(options)
    {
    }

    public DbSet<EventEntity> Events { get; set; }
  }
}