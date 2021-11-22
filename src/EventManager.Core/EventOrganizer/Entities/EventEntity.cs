using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventManager.Core.EventOrganizer.Basics;

namespace EventManager.Core.EventOrganizer.Entities
{
  public class EventEntity : BaseEntity<Guid>
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override Guid Id { get => base.Id; set => base.Id = value; }

    [Required(ErrorMessage = "Name of the event is must")]
    public string Name { get; set; }

    [MaxLength(100)]
    [Required(ErrorMessage = "Location of the event is must")]
    public string Location { get; set; }

    public uint Capacity { get; set; }

    public string Country { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; private set; }
  }
}