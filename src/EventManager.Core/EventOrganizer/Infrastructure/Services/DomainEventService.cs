using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Contracts;
using SharedKernel.Events;
using System;
using System.Threading.Tasks;

namespace EventOrganizer.Infrastructure.Services
{
  public class DomainEventService : IDomainEventService
  {
    private readonly ILogger<DomainEventService> _logger;
    private readonly IPublisher _mediator;

    public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
    {
      _logger = logger;
      _mediator = mediator;
    }

    public async Task Publish(BaseDomainEvent domainEvent)
    {
      _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
      await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private INotification GetNotificationCorrespondingToDomainEvent(BaseDomainEvent domainEvent)
    {
      return (INotification)Activator.CreateInstance(
          typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
    }

    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : BaseDomainEvent
    {
      public DomainEventNotification(TDomainEvent domainEvent)
      {
        DomainEvent = domainEvent;
      }

      public TDomainEvent DomainEvent { get; }
    }
  }
}