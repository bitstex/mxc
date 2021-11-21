using System.Threading;
using System.Threading.Tasks;

namespace EventManager.API.Common.Contracts
{
  public interface IApplicationDbContext
  {
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
}