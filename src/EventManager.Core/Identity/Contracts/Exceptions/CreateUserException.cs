using System;
using EventManager.Core.Identity.Models;

namespace EventManager.Core.Identity.Contracts.Exceptions
{
  /// <summary>
  /// Unknown error happend 
  /// </summary>
  public class CreateUserException : BaseIdentityException
  {
    public CreateUserException(AuthenticationModel credential) : base("Unknown error is occured", credential) { }
  }
}