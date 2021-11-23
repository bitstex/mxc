using System.ComponentModel.DataAnnotations;
using System;

namespace EventManager.Core.Identity.Models
{

  /// <summary>
  /// Wrapper class for the encrypted string token and date of the expiration
  /// </summary>
  public class TokenModel
  {
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; }

    public DateTime Expiration { get; set; }
  }
}