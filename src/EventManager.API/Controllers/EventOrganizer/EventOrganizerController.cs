using System;
using System.Threading;
using System.Threading.Tasks;
using EventManager.Core.EventOrganizer.Contracts.Interfaces;
using EventManager.Core.EventOrganizer.Entities;
using EventManager.Core.EventOrganizer.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers.EventOrganizer
{

  [Route("api/[controller]")]
  [ApiController]
  public class EventOrganizerController : ControllerBase
  {
    private readonly IRepository<EventEntity> _eventRepository;

    public EventOrganizerController(IRepository<EventEntity> eventRepository)
    {
      _eventRepository = eventRepository;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateEvent([FromBody] EditableEventModel model, CancellationToken cancellationToken)
    {
      var entity = await _eventRepository.AddAsync(new EventEntity
      {
        Name = model.Name,
        Location = model.Location,
        Capacity = model.Capacity,
        Country = model.Country,
        CreatedDate = DateTime.Now
      });
      return Ok(new { entity.Id, entity.CreatedDate });
    }

  }
}