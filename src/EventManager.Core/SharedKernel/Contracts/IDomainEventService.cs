using System.Threading.Tasks;
using SharedKernel.Events;

namespace SharedKernel.Contracts
{
  public interface IDomainEventService
  {
    Task Publish(BaseDomainEvent domainEvent);
  }
}