namespace SharedKernel.Contracts
{
  /// <summary>
  /// Use the IAggregateRoot interface to the domain object which encapsulates operations on multiple child objects.
  /// <remark>
  /// This interface is necessary to apply the repository pattern, but not necessary to solve the homework, becuse my solution does have
  /// only one entity without any child entities or value objects.
  /// </remark>
  /// </summary>
  public interface IAggregateRoot
  {
  }
}