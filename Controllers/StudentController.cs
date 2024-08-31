using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
		private readonly UserManager<ApplicationUser> _userManager;
		public StudentController(IStudent student, UserManager<ApplicationUser> userManager)
		{
			_student = student;
			_userManager = userManager;
		}

		
		[HttpGet("GetStudentById/{id}")]
		public async Task<IActionResult> GetStudentById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest("Invalid student ID");
			}

			var student = await _userManager.FindByIdAsync(id.ToString());

			if (student == null)
			{
				return NotFound("Student not found");
			}

			var studentDTO = new StudentDTO()
			{
				Id = student.Id,
				Email = student.Email,
				FullName = student.FullName,
				Cohort = student.Cohort,
				Course = student.Course
			};

			return Ok(studentDTO);
		}

		[HttpGet("GetAllStudents")]
		public async Task<IActionResult> GetAllStudents()
		{
			// Assuming "Student" is the role name for students
			var students = await _userManager.GetUsersInRoleAsync("Student");  /*GetUsersInRoleAsync("Student");*/

			if (students == null || !students.Any())
			{
				return NotFound("No students found");
			}

			// Map the students to a list of StudentDTOs
			var studentDTOs = students.Select(student => new StudentDTO
			{
				Id = student.Id,
				FullName = student.FullName,
				Email = student.Email,
				Cohort = student.Cohort,
				Course = student.Course

			}).ToList();

			return Ok(studentDTOs);
		}


		[HttpDelete("DeleteStudentBy{Id}")]
		public async Task<IActionResult> DeleteStudentById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest("Student ID cannot be null or empty.");
			}

			// Find the student by ID
			var student = await _userManager.FindByIdAsync(id);

			if (student == null)
			{
				return NotFound("The student was not found");
			}

			// Delete the student
			var result = await _userManager.DeleteAsync(student);
			if (!result.Succeeded)
			{
				return StatusCode(500, "An error occurred while deleting the student.");
			}

			return Ok("Student deleted successfully.");
		}

		//[HttpGet]
		//[Route("GetAllStudent")]
		//public IActionResult GetAllStudents()
		//{
		//	var students = _student.GetAllStudents();
		//	return Ok(students);
		//}

	}
	}
