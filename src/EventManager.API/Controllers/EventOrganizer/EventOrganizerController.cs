using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventManager.Core.EventOrganizer.Contracts.Interfaces;
using EventManager.Core.EventOrganizer.Entities;
using EventManager.Core.EventOrganizer.Models;
using EventManager.Core.EventOrganizer.Specifications;
using EventManager.Core.EventOrganizer.Specifications.Filters;
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
    public async Task<IActionResult> Post([FromBody] EditableEventModel model, CancellationToken cancellationToken)
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

    [HttpGet]
    public Task<List<EventEntity>> Get([FromQuery] EventFilter filter)
    {
      filter = filter ?? new EventFilter();

      // Here you can decide if you want the collections as well

      filter.IsPagingEnabled = true;

      return _eventRepository.ListAsync(new EventSpecification(filter));
    }

  }
}