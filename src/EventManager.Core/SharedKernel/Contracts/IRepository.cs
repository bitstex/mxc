using Ardalis.Specification;

namespace SharedKernel.Contracts
{

  /// <summary>
  /// This interface necessary to implement repository pattern without infrastructure dependency 
  /// </summary>
  /// <typeparam name="T">Type of the root of domain objects</typeparam>
  public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
  {
  }
}