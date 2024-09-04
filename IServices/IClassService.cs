using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface IClassService
	{
		Task<bool> ScheduleClassAsync(ClassSchedule classSchedule);
		Task<IEnumerable<ClassSchedule>> GetScheduledClassesAsync(string cohort);
		Task<IEnumerable<string>> GetCohortsAsync();
	}

	public interface IEventService
	{
		Task<bool> ScheduleEventAsync(IFormFile image, string eventLink);
		Task<IEnumerable<Event>> GetScheduledEventsAsync();
	}
}
