using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
	public class EventService : IEventService
	{
		private readonly LmsDbContext _dbContext;
		private readonly IWebHostEnvironment _environment;

		public EventService(LmsDbContext dbContext, IWebHostEnvironment environment)
		{
			_dbContext = dbContext;
			_environment = environment;
		}

		// Schedule an event with image and link
		public async Task<bool> ScheduleEventAsync(ScheduleEventDTO eventDTO)
		{
			if (eventDTO.Image == null || eventDTO.Image.Length == 0)
				throw new InvalidOperationException("Invalid image file.");

			var eventEntity = new Event
			{
				EventLink = eventDTO.EventLink
			};

			// Store the file data in the database
			using (var memoryStream = new MemoryStream())
			{
				await eventDTO.Image.CopyToAsync(memoryStream);
				eventEntity.FileData = memoryStream.ToArray();
			}

			_dbContext.Events.Add(eventEntity);
			await _dbContext.SaveChangesAsync();

			return true;
		}

		// Retrieve the scheduled events
		public async Task<IEnumerable<Event>> GetScheduledEventsAsync()
		{
			return await _dbContext.Events.ToListAsync();
		}

		// Implement the method to delete event by Id
		public async Task<bool> DeleteEventByIdAsync(int eventId)
		{
			var eventEntity = await _dbContext.Events.FindAsync(eventId);
			if (eventEntity == null)
			{
				return false; // Event not found
			}

			_dbContext.Events.Remove(eventEntity);
			await _dbContext.SaveChangesAsync();
			return true; // Event successfully deleted
		}
	}
}
