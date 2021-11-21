using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Contracts
{
  public interface IApplicationDbContext
  {
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
}