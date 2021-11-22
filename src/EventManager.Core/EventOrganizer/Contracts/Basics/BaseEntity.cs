using System;

namespace EventManager.Core.EventOrganizer.Basics
{
  /// <summary>
  /// Reusable base entity class for the arbitrary derived entity 
  /// </summary>
  /// <typeparam name="TId">Generic type of the ID of the derived entity</typeparam>
  public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
  {
    /// <summary>
    /// Unique identifier of the derived entity
    /// </summary>
    /// <value>Determines the entity object</value>
    public virtual TId Id { get; set; }
    /// <summary>
    /// Based on the Domain Driven Design principles
    /// </summary>
    /// <typeparam name="BaseDomainEvent">Base class of the derived domain events </typeparam>
    /// <returns>List of the occurred domain events</returns>

    #region Equality
    /// <summary>
    /// Override the general equals method of the object class
    /// </summary>
    /// <param name="obj">Object instance of the BaseEntity</param>
    /// <returns>The current entity and the other are the same or not</returns>
    public override bool Equals(object obj)
    {
      var entity = obj as BaseEntity<TId>;
      if (entity != null)
      {
        return this.Equals(entity);
      }
      return base.Equals(obj);
    }

    /// <summary>
    /// No hashcode override, no equals ...
    /// </summary>
    /// <returns>Hashcode of the custom Id</returns>
    public override int GetHashCode()
    {
      return this.Id.GetHashCode();
    }

    /// <summary>
    /// Two different entity equals are if the ids are also equals
    /// </summary>
    /// <param name="other">Other entity</param>
    /// <returns>The current entity and the other are the same or not</returns>
    public bool Equals(BaseEntity<TId> other)
    {
      if (other == null)
      {
        return false;
      }
      return this.Id.Equals(other.Id);
    }
    #endregion
  }
}