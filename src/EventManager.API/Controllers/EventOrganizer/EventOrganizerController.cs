using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManager.Core.EventOrganizer.Contracts.Interfaces;
using EventManager.Core.EventOrganizer.Entities;
using EventManager.Core.EventOrganizer.Models;
using EventManager.Core.EventOrganizer.Specifications;
using EventManager.Core.EventOrganizer.Specifications.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers.EventOrganizer
{

  [Route("api/v1/organizer/events")]
  [ApiController]
  public class EventOrganizerController : ControllerBase
  {
    private readonly IRepository<EventEntity> _eventRepository;

    public EventOrganizerController(IRepository<EventEntity> eventRepository)
    {
      _eventRepository = eventRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EditableEventModel model)
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


    [HttpPut]
    public async Task<IActionResult> Put([FromQuery] Guid id, [FromBody] EditableEventModel model)
    {
      if (id == Guid.Empty)
        return BadRequest("Id is empty");

      var entity = await _eventRepository.GetByIdAsync(id);
      if (entity == null)
        return NotFound();

      if (!String.IsNullOrEmpty(model.Country))
        entity.Country = model.Country;

      if (!String.IsNullOrEmpty(model.Name))
        entity.Name = model.Name;

      if (!String.IsNullOrEmpty(model.Location))
        entity.Location = model.Location;

      if (model.Capacity > 0)
        entity.Capacity = model.Capacity;

      await _eventRepository.UpdateAsync(entity);
      return Ok();
    }

    [HttpGet]
    public Task<List<EventEntity>> Get([FromQuery] EventFilter filter)
    {
      filter = filter ?? new EventFilter();
      return _eventRepository.ListAsync(new EventSpecification(filter));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
      if (id == Guid.Empty)
        return BadRequest("Id is empty");

      var entity = await _eventRepository.GetByIdAsync(id);
      if (entity == null)
        return NotFound();

      await _eventRepository.DeleteAsync(entity);
      return Ok();
    }

  }
}