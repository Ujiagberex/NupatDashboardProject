using Microsoft.AspNetCore.Mvc;
using Nest;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : ControllerBase
	{
		private readonly IStudent _student;
		public StudentController(IStudent student)
		{
			_student = student;
		}
		
		[HttpPost]
		[Route("CreateStudent")]
		public IActionResult AddStudent(AddStudentDTO student)
		{
			_student.AddStudent(student);

			return Ok("successful");
		}
		
		[HttpDelete("DeleteStudentBy{Id}")]
		public IActionResult DeleteStudentById(Guid id)
		{
			bool student = _student.DeleteStudentById(id);
			if (student == null)
			{
				return BadRequest();
			}
			return Ok("successful");
		}

		[HttpGet("GetStudentBy{id}")]
		public IActionResult GetStudentById(Guid id)
		{
			var FindStudent = _student.GetStudentById(id);
			if (FindStudent == null)
			{
				return BadRequest("student does not exist");
			}
			return Ok(FindStudent);
		}

		[HttpGet]
		[Route("GetAllStudent")]
		public IActionResult GetAllStudents()
		{
			var students = _student.GetAllStudents();
			return Ok(students);
		}

		[HttpPut("UpdateStudentBy{Id}")]
		public IActionResult UpdateStudentById(Student student)
		{
			var update = _student.UpdateStudentById(student);
			if (update == null)
			{
				return NotFound();
			}
			return Ok(update);

		}
		}
	}
