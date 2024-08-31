using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Controllers
{
	[Route("api/Courses")]
	[ApiController]
	public class CourseController : ControllerBase
	{

		private readonly LmsDbContext _dbContext;
		private readonly ICourse _course;
		private readonly ICourseRepository _courseRepository;


		public CourseController(LmsDbContext dbContext, ICourse course,ICourseRepository courseRepository)
		{
			_dbContext = dbContext;
			_course = course;
			_courseRepository = courseRepository;
		}

		[HttpPost("Create")]
		public IActionResult AddCourse(AddCourseDTO course)
		{
			_course.AddCourse(course);

			return Ok("successful");
		}

		[HttpGet("GetAllCourses")]
		public IActionResult GetAll()
		{
			var courses = _dbContext.Courses.ToList();
			return Ok(courses);
		}

		[HttpGet("/GetCourseBy{id}")]
		public IActionResult GetById([FromRoute] Guid id)
		{
			var course = _dbContext.Courses.Find(id);
			if (course == null)
			{
				return NotFound();
			}

			return Ok(course);
		}

		[HttpDelete("DeleteCourseBy{Id}")]
		public IActionResult CourseById(Guid id)
		{
			bool course = _course.DeleteCourseById(id);

			if (course == null)
			{
				return BadRequest("Course was not found");
			}
			return Ok("successful");
		}


		[HttpGet("UpcomingTest")]
		public async Task<ActionResult<IEnumerable<Test>>> GetUpcomingTests()
		{
			var upcomingTests = await _dbContext.Tests.Where(t => t.TestDate >= DateTime.Now).OrderBy(t => t.TestDate).ToListAsync();

			return Ok(upcomingTests);
		}


		[HttpGet("OverallAttendace")]
		public async Task<ActionResult<OverallAttendanceDTO>> GetOverallAttendance()
		{
			var totalStudents = await _dbContext.Attendances.GroupBy(a => a.StudentId).CountAsync();
			var totalPresent = await _dbContext.Attendances.Where(a => a.IsPresent).CountAsync();
			var totalAbsent = totalStudents - totalPresent;

			var overallAttendance = new OverallAttendanceDTO
			{
				TotalStudents = totalStudents,
				TotalPresent = totalPresent,
				TotalAbsent = totalAbsent
			};

			return Ok(overallAttendance);
		}

		[HttpGet("PopularCourses")]
		public async Task<ActionResult<IEnumerable<Course>>> GetMostPopularCourses([FromQuery] int limit = 10)
		{
			var courses = await _courseRepository.GetMostPopularCoursesAsync(limit);
			return Ok(courses);
		}


	}
}