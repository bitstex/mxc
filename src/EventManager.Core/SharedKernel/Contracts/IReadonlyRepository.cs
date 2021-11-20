
using Ardalis.Specification;

namespace SharedKernel.Contracts
{
  /// <summary>
  /// This interface support to implement the read model of domain models
  /// </summary>
  /// <typeparam name="T">Type of the root of domain objects</typeparam>
  public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
  {
  }

}