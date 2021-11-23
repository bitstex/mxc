using System;

namespace EventManager.Core.EventOrganizer.Specifications.Filters
{
  public class EventFilter : PagingFilter
  {
    public Guid? Id { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public uint? Capacity { get; set; }

    public string Country { get; set; }
  }
}