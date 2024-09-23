using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<ApplicationUser> _userManager;


		public CourseController(LmsDbContext dbContext, ICourse course,ICourseRepository courseRepository, UserManager<ApplicationUser> userManager)
		{
			_dbContext = dbContext;
			_course = course;
			_courseRepository = courseRepository;
			_userManager = userManager;
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

		[HttpGet("GetCourseBy{id}")]
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

		// GET: api/Assignments/download/{assignmentId}
		[HttpGet("downloadAssignment/{assignmentId}")]
		public async Task<IActionResult> DownloadAssignment(Guid assignmentId)
		{
			// Retrieve the assignment from the database
			var assignment = await _dbContext.Assignments.FindAsync(assignmentId);
			if (assignment == null)
			{
				return NotFound("Assignment not found.");
			}

			// Check if the assignment has a file attached
			if (assignment.FileData == null || assignment.FileData.Length == 0)
			{
				return NotFound("No file available for this assignment.");
			}

			// Set the content type based on the file extension (optional, for better browser support)
			var contentType = "application/octet-stream"; // Default binary stream
			var extension = Path.GetExtension(assignment.FilePath)?.ToLowerInvariant();
			if (!string.IsNullOrEmpty(extension))
			{
				contentType = extension switch
				{
					".pdf" => "application/pdf",
					".doc" or ".docx" => "application/msword",
					".xls" or ".xlsx" => "application/vnd.ms-excel",
					".jpg" or ".jpeg" => "image/jpeg",
					".png" => "image/png",
					_ => "application/octet-stream",
				};
			}

			// Return the file for download
			var fileStream = new MemoryStream(assignment.FileData);
			return File(fileStream, contentType, assignment.FilePath);
		}

		[HttpDelete("DeleteSubmittedAssignmentById")]
		public async Task<IActionResult> DeleteSubmittedAssignmentById(Guid id)
		{
			var submitedAssignment = await _dbContext.SubmittedAssignments.FindAsync(id);
			if (submitedAssignment == null)
			{
				return NotFound("No Submitted Assignment");
			}

			_dbContext.SubmittedAssignments.Remove(submitedAssignment);
			await _dbContext.SaveChangesAsync();

			return Ok("Submitted Assignment deleted successfully");
		}

		// POST: api/Assignments/submit
		[HttpPost("SubmitAssignment")]
		public async Task<IActionResult> SubmitAssignment([FromForm] SubmitAssignmentDTO submitAssignmentDTO)
		{
			long maxFileSize = 100 * 1024 * 1024; // 100 MB in bytes

			if (submitAssignmentDTO.File == null || submitAssignmentDTO.File.Length == 0)
				return BadRequest("No file uploaded.");

			if (submitAssignmentDTO.File.Length > maxFileSize)
			{
				return BadRequest("File size exceeds the maximum allowed limit of 100 MB.");
			}

			if (string.IsNullOrWhiteSpace(submitAssignmentDTO.StudentId))
			{
				return BadRequest("Student ID is required.");
			}

			if (submitAssignmentDTO.AssignmentId == Guid.Empty)
			{
				return BadRequest("Assignment ID is required.");
			}

			
			// Check if the assignment exists
			var assignment = await _dbContext.Assignments.FindAsync(submitAssignmentDTO.AssignmentId);
			if (assignment == null)
			{
				return NotFound("Assignment not found.");
			}

			//Check if student Exists
			var student = await _userManager.FindByIdAsync(submitAssignmentDTO.StudentId);
			if (student == null)
			{
				return NotFound("Student not found.");
			}

			// Check if the assignment is still open for submission
			if (assignment.DueDate < DateTime.Now)
			{
				return BadRequest("The assignment submission deadline has passed.");
			}

			var existingSubmission = await _dbContext.SubmittedAssignments
			.FirstOrDefaultAsync(sa => sa.AssignmentId == submitAssignmentDTO.AssignmentId && sa.Id == submitAssignmentDTO.StudentId);

			if (existingSubmission != null)
			{
				return BadRequest("Assignment has already been submitted by this student.");
			}

			var submittedAssignment = new SubmitAssignment
			{
				AssignmentId = submitAssignmentDTO.AssignmentId,
				Id = student.Id, // Assuming StudentId is part of the DTO
				DueDate = assignment.DueDate,
				DateUploaded = assignment.DateUploaded,
				SubmissionDate = DateTime.Now,
				Status = "Submitted",
				FilePath = submitAssignmentDTO.File.FileName
			};

			// Store the file data
			using (var memoryStream = new MemoryStream())
			{
				await submitAssignmentDTO.File.CopyToAsync(memoryStream);
				submittedAssignment.FileData = memoryStream.ToArray();
			}

			_dbContext.SubmittedAssignments.Add(submittedAssignment);
			await _dbContext.SaveChangesAsync();

			return Ok("Assignment submitted successfully.");
		}


	}
}