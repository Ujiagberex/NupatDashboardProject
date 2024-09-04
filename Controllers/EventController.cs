using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.IServices;

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

		//Post Event
		[HttpPost("Schedule")]
		public async Task<IActionResult> ScheduleEvent([FromForm] IFormFile image, [FromForm] string eventLink)
		{
			var result = await _eventService.ScheduleEventAsync(image, eventLink);
			return result ? Ok("Event scheduled successfully") : StatusCode(500, "Failed to schedule event");
		}

		//Get events
		[HttpGet("Events")]
		public async Task<IActionResult> GetScheduledEvents()
		{
			var events = await _eventService.GetScheduledEventsAsync();
			return Ok(events);
		}
	}
}
