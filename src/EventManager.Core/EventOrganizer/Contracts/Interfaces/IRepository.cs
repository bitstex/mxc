using Ardalis.Specification;

namespace EventManager.Core.EventOrganizer.Contracts.Interfaces
{

  /// <summary>
  /// This interface necessary to implement repository pattern
  /// </summary>
  /// <typeparam name="T">Type of the root of domain objects</typeparam>
  public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
  {
  }

  /// <summary>
  /// This readonly repository
  /// </summary>
  /// <typeparam name="T">Type of the root of domain objects</typeparam>
  public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
  {
  }
}