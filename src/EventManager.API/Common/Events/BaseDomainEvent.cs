using System;
using MediatR;

namespace EventManager.API.Common.Events
{
  /// <summary>
  /// The minimum base class of domain events based on DDD principles and the mediator pattern 
  /// </summary>
  public abstract class BaseDomainEvent : INotification
  {
    /// <summary>
    /// Help to determine the order of events and also support to create the audit logs
    /// </summary>
    /// <value>Time of the event creation</value>
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
  }
}