using NupatDashboardProject.DTO;
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
		Task<bool> ScheduleEventAsync(ScheduleEventDTO eventDTO);
		Task<IEnumerable<Event>> GetScheduledEventsAsync();
		Task<bool> DeleteEventByIdAsync(int eventId);
	}
}
