using Ardalis.Specification.EntityFrameworkCore;
using EventManager.Core.EventOrganizer.Contracts.Interfaces;
using EventManager.Core.EventOrganizer.Entities;

namespace EventManager.Infrastructure.Identity.DataContext
{
  public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
  {
    private readonly EventOrganizerDbContext dbContext;

    public EfRepository(EventOrganizerDbContext dbContext) : base(dbContext)
    {
      this.dbContext = dbContext;
    }

  }
}