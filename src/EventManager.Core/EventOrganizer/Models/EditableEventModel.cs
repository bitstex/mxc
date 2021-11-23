using System.ComponentModel.DataAnnotations;

namespace EventManager.Core.EventOrganizer.Models
{
  /// <summary>
  /// Editable properties of the event entity
  /// </summary>
  public class EditableEventModel
  {

    [Required(ErrorMessage = "Name of the event is must")]
    public string Name { get; set; }

    [MaxLength(100)]
    [Required(ErrorMessage = "Location of the event is must")]
    public string Location { get; set; }

    public ushort? Capacity { get; set; }

    public string Country { get; set; }
  }
}