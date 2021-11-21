using System.Threading.Tasks;
using EventManager.API.Common.Events;

namespace EventManager.API.Common.Contracts
{
  public interface IDomainEventService
  {
    Task Publish(BaseDomainEvent domainEvent);
  }
}