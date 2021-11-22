using System.ComponentModel.DataAnnotations;
using System;

namespace EventManager.Core.Identity.Models
{

  public class TokenModel
  {
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; }

    public DateTime Expiration { get; set; }
  }
}