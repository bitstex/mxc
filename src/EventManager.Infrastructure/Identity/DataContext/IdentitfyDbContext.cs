using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.Identity.DataContext
{
  public class IdentitfyDbContext : IdentityDbContext<ApplicationUser>
  {
    public IdentitfyDbContext(DbContextOptions<IdentitfyDbContext> options) : base(options)
    {
    }
  }
}