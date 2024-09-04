using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
	public class ClassService : IClassService
	{
		private readonly LmsDbContext _context;

		public ClassService(LmsDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<string>> GetCohortsAsync()
		{
			// Fetch distinct cohort names from the database
			return await _context.ClassSchedules
								 .Select(c => c.Cohort)
								 .Distinct()
								 .ToListAsync();
		}

		public async Task<IEnumerable<ClassSchedule>> GetScheduledClassesAsync(string cohort)
		{
			// Fetch all classes for the specified cohort
			return await _context.ClassSchedules
								 .Where(c => c.Cohort == cohort)
								 .ToListAsync();
		}

		public async Task<bool> ScheduleClassAsync(ClassSchedule classSchedule)
		{
			try
			{
				// Add the new class schedule to the database
				await _context.ClassSchedules.AddAsync(classSchedule);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				// Handle or log the exception as needed
				return false;
			}
		}
	}
}
