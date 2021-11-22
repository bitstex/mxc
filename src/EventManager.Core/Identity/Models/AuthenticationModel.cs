using System.ComponentModel.DataAnnotations;

namespace EventManager.Core.Identity.Models
{

  public class AuthenticationModel
  {
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
  }
}