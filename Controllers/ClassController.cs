using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClassController : ControllerBase
	{
		private readonly IClassService _classService;

		public ClassController(IClassService classService)
		{
			_classService = classService;
		}

		[HttpPost("CreateSchedule")]
		public async Task<IActionResult> ScheduleClass([FromBody] ClassSchedule classSchedule)
		{
			var result = await _classService.ScheduleClassAsync(classSchedule);
			return result ? Ok("Class scheduled successfully") : StatusCode(500, "Failed to schedule class");
		}

		[HttpGet("cohorts")]
		public async Task<IActionResult> GetCohorts()
		{
			var cohorts = await _classService.GetCohortsAsync();
			return Ok(cohorts);
		}

		[HttpGet("GetScheduled-classes")]
		public async Task<IActionResult> GetScheduledClasses([FromQuery] string cohort)
		{
			var classes = await _classService.GetScheduledClassesAsync(cohort);
			return Ok(classes);
		}
	}
}
