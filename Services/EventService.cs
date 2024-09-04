using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
	public class EventService : IEventService
	{
		private readonly LmsDbContext _context;
		private readonly IWebHostEnvironment _environment;

		public EventService(LmsDbContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		public async Task<bool> ScheduleEventAsync(IFormFile image, string eventLink)
		{
			try
			{
				// Save the image to a folder
				var imagePath = await SaveImageAsync(image);

				// Create a new event
				var newEvent = new Event
				{
					ImagePath = imagePath,
					EventLink = eventLink,
					Date = DateTime.Now, // Assuming event date is now or use a provided date
					Time = DateTime.Now.TimeOfDay // Assuming event time is now or use a provided time
				};

				// Save the event to the database
				await _context.Events.AddAsync(newEvent);
				await _context.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				// Handle or log the exception as needed
				return false;
			}
		}

		public async Task<IEnumerable<Event>> GetScheduledEventsAsync()
		{
			// Fetch all events from the database
			return await _context.Events.ToListAsync();
		}

		private async Task<string> SaveImageAsync(IFormFile image)
		{
			// Check if the image is not null and has content
			if (image == null || image.Length == 0)
			{
				throw new ArgumentException("Image file is not valid.");
			}

			// Create a unique file name
			var fileName = Path.GetFileNameWithoutExtension(image.FileName);
			var extension = Path.GetExtension(image.FileName);
			var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

			// Define the path to save the file
			var path = Path.Combine(_environment.WebRootPath, "uploads", uniqueFileName);

			// Save the file to the specified path
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await image.CopyToAsync(fileStream);
			}

			return Path.Combine("uploads", uniqueFileName);
		}
	}
}
