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
		
		private readonly UserManager<ApplicationUser> _userManager;
		public StudentController( UserManager<ApplicationUser> userManager)
		{
			
			_userManager = userManager;
		}

		//Get Student by Id
		[HttpGet("GetStudentBy{id}")]
		public async Task<IActionResult> GetStudentById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest("Invalid student ID");
			}

			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound("Student not found");
			}

            // Cast to Student
            var student = user as Student;

            if (student == null)
            {
                return BadRequest("User is not a student");
            }

            var studentDTO = new StudentDTO()
			{
				Id = student.Id,
				Email = student.Email,
				FullName = student.FullName,
				CohortId = student.CohortId,
				CourseId = student.CourseId
			};

			return Ok(studentDTO);
		}

		//Get all Students 
		[HttpGet("GetAllStudents")]
		public async Task<IActionResult> GetAllStudents()
		{
			// Assuming "Student" is the role name for students
			var usersInRole = await _userManager.GetUsersInRoleAsync("Student");

			if (usersInRole == null || !usersInRole.Any())
			{
				return NotFound("No students found");
			}

            // Filter and cast users to Student
            var students = usersInRole.OfType<Student>().ToList();

            // Map the students to a list of StudentDTOs
            var studentDTOs = students.Select(student => new StudentDTO
			{
				Id = student.Id,
				FullName = student.FullName,
				Email = student.Email,
				CohortId = student.CohortId,
				CourseId = student.CourseId

			}).ToList();

			return Ok(studentDTOs);
		}

		//Delete Student by Id
		[HttpDelete("DeleteStudentBy{id}")]
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
