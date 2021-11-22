using System.ComponentModel.DataAnnotations;

namespace EventManager.Core.EventOrganizer.Models
{
  public class EditableEventModel
  {
    [Required(ErrorMessage = "Name of the event is must")]
    public string Name { get; set; }

    [MaxLength(100)]
    [Required(ErrorMessage = "Location of the event is must")]
    public string Location { get; set; }

    public uint Capacity { get; set; }

    public string Country { get; set; }
  }
}