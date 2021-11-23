using EventManager.Core.EventOrganizer.Specifications.Filters;

namespace EventManager.Core.EventOrganizer.Specifications
{
  /// <summary>
  /// Helper class to support paging on the server side 
  /// </summary>
  public static class PaginationHelper
  {

    public static int DefaultPage => 1;
    public static int DefaultPageSize => 10;

    public static int CalculateTake(int pageSize)
    {
      return pageSize <= 0 ? DefaultPageSize : pageSize;
    }
    public static int CalculateSkip(int pageSize, int page)
    {
      page = page <= 0 ? DefaultPage : page;

      return CalculateTake(pageSize) * (page - 1);
    }

    public static int CalculateTake(PagingFilter baseFilter)
    {
      return CalculateTake(baseFilter.PageSize);
    }
    public static int CalculateSkip(PagingFilter baseFilter)
    {
      return CalculateSkip(baseFilter.PageSize, baseFilter.Page);

    }
  }
}