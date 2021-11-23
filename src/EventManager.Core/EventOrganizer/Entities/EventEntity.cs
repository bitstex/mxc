using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventManager.Core.EventOrganizer.Basics;
using EventManager.Core.EventOrganizer.Contracts.Interfaces;

namespace EventManager.Core.EventOrganizer.Entities
{
  public class EventEntity : BaseEntity<Guid>, IAggregateRoot
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override Guid Id { get => base.Id; set => base.Id = value; }

    [Required(ErrorMessage = "Name of the event is must")]
    public string Name { get; set; }

    [MaxLength(100)]
    [Required(ErrorMessage = "Location of the event is must")]
    public string Location { get; set; }

    public ushort? Capacity { get; set; }

    public string Country { get; set; }

    public DateTime CreatedDate { get; set; }
  }
}