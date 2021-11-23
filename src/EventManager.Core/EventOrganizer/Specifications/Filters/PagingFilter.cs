namespace EventManager.Core.EventOrganizer.Specifications.Filters
{
  /// <summary>
  /// Filter model of the paging
  /// </summary>
  public class PagingFilter
  {
    public bool IsPagingEnabled { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
  }
}
