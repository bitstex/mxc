using Ardalis.Specification;
using EventManager.Core.EventOrganizer.Entities;
using EventManager.Core.EventOrganizer.Specifications.Filters;

namespace EventManager.Core.EventOrganizer.Specifications
{
  public class EventSpecification : Specification<EventEntity>
  {
    public EventSpecification(EventFilter filter)
    {
      Query.OrderBy(x => x.Name)
                 .ThenByDescending(x => x.CreatedDate);


      if (filter.IsPagingEnabled)
        Query.Skip(PaginationHelper.CalculateSkip(filter))
             .Take(PaginationHelper.CalculateTake(filter));

      if (!string.IsNullOrEmpty(filter.Name))
        Query.Where(x => x.Name == filter.Name);

      if (!string.IsNullOrEmpty(filter.Country))
        Query.Where(x => x.Country == filter.Country);

      if (!string.IsNullOrEmpty(filter.Location))
        Query.Search(x => x.Location, "%" + filter.Location + "%");

      if (filter.Capacity > 0)
        Query.Where(x => x.Capacity == filter.Capacity);

    }
  }
}