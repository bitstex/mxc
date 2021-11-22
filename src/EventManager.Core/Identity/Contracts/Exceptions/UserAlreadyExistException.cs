using System;
using EventManager.Core.Identity.Models;

namespace EventManager.Core.Identity.Contracts.Exceptions
{
  public class UserAlreadyExistException : BaseIdentityException
  {
    public UserAlreadyExistException(AuthenticationModel credential) : base("The user is already registered", credential) { }
  }
}