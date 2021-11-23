using System;
using EventManager.Core.Identity.Models;

namespace EventManager.Core.Identity.Contracts.Exceptions
{
  /// <summary>
  /// Base error class to handle domain errors
  /// </summary>
  public abstract class BaseIdentityException : Exception
  {
    public String UserName { get; private set; }

    public BaseIdentityException(string message, AuthenticationModel credential) : base(message)
    {
      UserName = credential.Username;
    }
  }
}