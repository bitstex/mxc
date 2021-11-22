namespace EventManager.Core.EventOrganizer.Specifications.Filters
{
  public class PagingFilter
  {
    public bool IsPagingEnabled { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
  }
}
