using System.ComponentModel.DataAnnotations;

namespace EventManager.Core.EventOrganizer.Models
{
  public class EditableEventModel
  {
    public string Name { get; set; }

    public string Location { get; set; }

    public uint Capacity { get; set; }

    public string Country { get; set; }
  }
}