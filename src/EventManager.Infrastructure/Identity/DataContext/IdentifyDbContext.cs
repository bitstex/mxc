using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.Identity.DataContext
{
  /// <summary>
  /// IdentityDb class to support EF operations
  /// </summary>
  public class IdentifyDbContext : IdentityDbContext<ApplicationUser>
  {
    public IdentifyDbContext(DbContextOptions<IdentifyDbContext> options) : base(options)
    {
    }
  }
}