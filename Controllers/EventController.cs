using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventController : ControllerBase
	{
		private readonly IEventService _eventService;

		public EventController(IEventService eventService)
		{
			_eventService = eventService;
		}

		// POST: api/event/schedule
		[HttpPost("schedule")]
		public async Task<IActionResult> ScheduleEvent([FromForm] ScheduleEventDTO eventDTO)
		{
			if (eventDTO.Image == null || string.IsNullOrEmpty(eventDTO.EventLink))
				return BadRequest("Invalid event data.");

			var result = await _eventService.ScheduleEventAsync(eventDTO);

			if (!result)
				return StatusCode(500, "Error scheduling the event.");

			return Ok("Event scheduled successfully.");
		}

		// GET: api/event
		[HttpGet]
		public async Task<IActionResult> GetScheduledEvents()
		{
			var events = await _eventService.GetScheduledEventsAsync();
			return Ok(events);
		}

		// DELETE: api/event/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEventById(int id)
		{
			var result = await _eventService.DeleteEventByIdAsync(id);

			if (!result)
			{
				return NotFound("Event not found.");
			}

			return Ok("Event deleted successfully.");
		}
	}
}
