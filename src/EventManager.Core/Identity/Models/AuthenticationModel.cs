using System.ComponentModel.DataAnnotations;

namespace EventManager.Core.Identity.Models
{
  /// <summary>
  /// Minimal credentials to request a JWT access token
  /// </summary>
  public class AuthenticationModel
  {
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
  }
}