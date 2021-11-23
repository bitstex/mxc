using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManager.Core.EventOrganizer.Contracts.Interfaces;
using EventManager.Core.EventOrganizer.Entities;
using EventManager.Core.EventOrganizer.Models;
using EventManager.Core.EventOrganizer.Specifications;
using EventManager.Core.EventOrganizer.Specifications.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventManager.API.Controllers.EventOrganizer
{
  /// <summary>
  /// Controller class of the http requests
  /// </summary>
  [Route("api/v1/organizer/events")]
  [ApiController]
  public class EventOrganizerController : ControllerBase
  {
    /// <summary>
    /// Repository
    /// </summary>
    private readonly IRepository<EventEntity> _eventRepository;
    /// <summary>
    /// Logger created by DI
    /// </summary>
    private readonly ILogger<EventOrganizerController> _logger;

    /// <summary>
    /// Construct the controller
    /// </summary>
    /// <param name="eventRepository"></param>
    public EventOrganizerController(IRepository<EventEntity> eventRepository, ILogger<EventOrganizerController> logger)
    {
      _eventRepository = eventRepository;
      _logger = logger;
    }

    /// <summary>
    /// Create a new event based on the require properties
    /// </summary>
    /// <param name="model"></param>
    /// <returns>The id of the new event and the date of the creation</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EditableEventModel model)
    {
      _logger.LogTrace(String.Format("HTTP Post request is received with: {0}", model));
      var entity = await _eventRepository.AddAsync(new EventEntity
      {
        Name = model.Name,
        Location = model.Location,
        Capacity = model.Capacity,
        Country = model.Country,
        CreatedDate = DateTime.Now
      });
      _logger.LogDebug(String.Format("entity: {0}", entity));
      _logger.LogInformation("Event is created");
      return Ok(new { entity.Id, entity.CreatedDate });
    }


    /// <summary>
    /// Modify the already exist event based on the creation rules
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Put([FromQuery] Guid id, [FromBody] EditableEventModel model)
    {
      _logger.LogTrace(String.Format("HTTP Put request is received with id: {0}, model: {1} ", id, model));

      if (id == Guid.Empty)
        return BadRequest("Id is empty");

      _logger.LogDebug("Id isn't empty");

      var entity = await _eventRepository.GetByIdAsync(id);
      _logger.LogDebug(String.Format("entity(old): {0}", entity));

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

      _logger.LogDebug(String.Format("entity(new): {0}", entity));

      await _eventRepository.UpdateAsync(entity);
      _logger.LogInformation("Event is updated");
      return Ok();
    }

    /// <summary>
    /// Advanced query of the all events
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<EventEntity>> Get([FromQuery] EventFilter filter)
    {
      _logger.LogTrace(String.Format("HTTP Get request is received with: {0}", filter));

      filter = filter ?? new EventFilter();
      var results = _eventRepository.ListAsync(new EventSpecification(filter));
      _logger.LogDebug(String.Format("results: {0}", results));
      _logger.LogInformation("All events are filtered");
      return results;
    }

    /// <summary>
    /// Soft delete the certain event 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
      _logger.LogTrace(String.Format("HTTP Delete request is received with id: {0}", id));
      if (id == Guid.Empty)
        return BadRequest("Id is empty");

      var entity = await _eventRepository.GetByIdAsync(id);
      _logger.LogDebug(String.Format("entity: {0}", entity));

      if (entity == null)
        return NotFound();

      await _eventRepository.DeleteAsync(entity);
      _logger.LogInformation("Event is deleted");
      return Ok();
    }

  }
}