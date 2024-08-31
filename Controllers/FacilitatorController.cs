using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;
using ServiceStack.Text;

namespace NupatDashboardProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FacilitatorController : ControllerBase
	{
		
		private readonly LmsDbContext _context;
		private readonly IFacilitator _facilitator;
		

		public FacilitatorController(LmsDbContext context, IFacilitator facilitator)
		{
			_context = context;
			_facilitator = facilitator;
			
		}

		//Upload Content By Facilitator
		[HttpPost("UploadContent")]
		public async Task<IActionResult> UploadContent([FromForm] UploadContentDTO uploadContentDTO)
		{
			if (uploadContentDTO.File == null || uploadContentDTO.File.Length == 0)
				return BadRequest("No file uploaded.");

			using (var memoryStream = new MemoryStream())
			{
				await uploadContentDTO.File.CopyToAsync(memoryStream);
				var content = new Content
				{
					ContentId = Guid.NewGuid(),
					FileName = uploadContentDTO.File.FileName,
					FileData = memoryStream.ToArray(),
					Owner = uploadContentDTO.Owner,
					UploadDate = DateTime.Now
				};

				_context.Contents.Add(content);
				await _context.SaveChangesAsync();
			}

			return Ok( "Content uploaded successfully.");

		}

		//Delete Content 
		[HttpDelete("DeleteContentBy{id}")]
		public async Task<IActionResult> DeleteContent(Guid id)
		{
			var content = await _context.Contents.FindAsync(id);

			if (content == null)
			{
				return NotFound();
			}

			_context.Contents.Remove(content);
			await _context.SaveChangesAsync();

			return Ok("Successfully Deleted");
		}
		
		//Get all content
		[HttpGet("GetAllContent")]
		public async Task<ActionResult<IEnumerable<Content>>> GetContents()
		{
			return await _context.Contents.ToListAsync();
		}

		//Get Uploaded Content by Id 
		[HttpGet("GetContentBy{id}")]
		public async Task<IActionResult> GetAssignmentById(Guid id)
		{
			var content = await _context.Contents.FindAsync(id);

			if (content == null)
			{
				return NotFound();
			}

			return Ok(content);
		}

		[HttpPut("UpdateContentBy{id}")]
		public async Task<IActionResult> UpdateContent(Guid id, [FromForm] UpdateContentDTO updateContentDTO)
		{
			// Find the existing content by its ID
			var content = await _context.Contents.FindAsync(id);

			// Check if the content exists
			if (content == null)
			{
				return NotFound(new { message = "Content not found." });
			}

			// Update the content properties
			if (updateContentDTO.File != null && updateContentDTO.File.Length > 0)
			{
				using (var memoryStream = new MemoryStream())
				{
					await updateContentDTO.File.CopyToAsync(memoryStream);
					// Optionally, store or process the file content
				}

				content.FileName = updateContentDTO.File.FileName;
			}

			content.Owner = updateContentDTO.Owner;
			content.UploadDate = DateTime.Now; // Update the upload date

			try
			{
				// Save the updated content to the database
				_context.Contents.Update(content);
				await _context.SaveChangesAsync();

				return Ok(new { message = "Content updated successfully.", content });
			}
			catch (Exception ex)
			{
				// Handle any errors that may occur
				return StatusCode(500,"An error occurred while updating the content.");
			}
		}

		//Upload Assignment
		[HttpPost("uploadAssignment")]
		public async Task<IActionResult> UploadAssignment([FromForm] UploadAssignmentDTO uploadAssignmentDTO)
		{
			long maxFileSize = 100 * 1024 * 1024; // 100 MB in bytes

			if (uploadAssignmentDTO.File == null || uploadAssignmentDTO.File.Length == 0)
				return BadRequest("No file uploaded.");
			if(uploadAssignmentDTO.File.Length > maxFileSize)
			{
				throw new InvalidOperationException("File size exceeds the maximum allowed limit of 100 MB.");
			}
			using (var memoryStream = new MemoryStream())
			{
				await uploadAssignmentDTO.File.CopyToAsync(memoryStream);
				var assignment = new Assignment
				{
					AssignmentId = Guid.NewGuid(),
					DateUploaded = DateTime.Now,
					DueDate = uploadAssignmentDTO.Duedate,
					FileData = memoryStream.ToArray(),
					FilePath = uploadAssignmentDTO.File.FileName,

				};

				_context.Assignments.Add(assignment);
				await _context.SaveChangesAsync();
			}

			return Ok("Content uploaded successfully.");
		}

		// POST: api/Assignments/submit
		[HttpPost("submitAssignment")]
		public async Task<IActionResult> SubmitAssignment([FromForm] SubmitAssignmentDTO submitAssignmentDTO)
		{
			long maxFileSize = 100 * 1024 * 1024; // 100 MB in bytes

			if (submitAssignmentDTO.File == null || submitAssignmentDTO.File.Length == 0)
				return BadRequest("No file uploaded.");

			if (submitAssignmentDTO.File.Length > maxFileSize)
				throw new InvalidOperationException("File size exceeds the maximum allowed limit of 100 MB.");

			// Check if the assignment exists
			var assignment = await _context.Assignments.FindAsync(submitAssignmentDTO.AssignmentId);
			if (assignment == null)
			{
				return NotFound("Assignment not found.");
			}

			// Check if the assignment is still open for submission
			if (assignment.DueDate < DateTime.Now)
			{
				return BadRequest("The assignment submission deadline has passed.");
			}

			// Store the file data and update assignment status
			using (var memoryStream = new MemoryStream())
			{
				await submitAssignmentDTO.File.CopyToAsync(memoryStream);
				assignment.FileData = memoryStream.ToArray();
				assignment.SubmissionDate = DateTime.Now;
				assignment.Status = "Submitted"; // Or any other status you'd like to set
				assignment.FilePath = submitAssignmentDTO.File.FileName; // Assuming FilePath stores the file name
			}

			_context.Assignments.Update(assignment);
			await _context.SaveChangesAsync();

			return Ok("Assignment submitted successfully.");
		}

		// GET: api/Assignments/download/{assignmentId}
		[HttpGet("downloadAssignment/{assignmentId}")]
		public async Task<IActionResult> DownloadAssignment(Guid assignmentId)
		{
			// Retrieve the assignment from the database
			var assignment = await _context.Assignments.FindAsync(assignmentId);
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

		//Get all student assignments
		[HttpGet("GetAllAssignments")]
		public async Task<IActionResult> GetAssignments()
		{
			var assignments = await _context.Assignments.ToListAsync();
			return Ok(assignments);
		}

		//Get assignment by Id
		[HttpGet("GetAssignmentBy{id}")]
		public async Task<IActionResult> GetContentById(Guid id)
		{
			var assignment = await _context.Assignments.FindAsync(id);

			if (assignment == null)
			{
				return NotFound();
			}

			return Ok(assignment);
		}

		//Update assignment
		[HttpPut("UpdateAssignmentBy{id}")]
		public async Task<IActionResult> UpdateAssignment(Guid id, [FromForm] UpdateAssignmentDTO updateAssignmentDTO)
		{
			long maxFileSize = 100 * 1024 * 1024; // 100 MB in bytes

			// Find the existing assignment by its ID
			var assignment = await _context.Assignments.FindAsync(id);

			// Check if the assignment exists
			if (assignment == null)
			{
				return NotFound(new { message = "Assignment not found." });
			}

			// Update the assignment properties
			if (updateAssignmentDTO.File != null && updateAssignmentDTO.File.Length > 0)
			{
				if (updateAssignmentDTO.File.Length > maxFileSize)
				{
					return BadRequest(new { message = "File size exceeds the maximum allowed limit of 100 MB." });
				}

				using (var memoryStream = new MemoryStream())
				{
					await updateAssignmentDTO.File.CopyToAsync(memoryStream);
					// Optionally, store or process the file content
				}

				assignment.FilePath = updateAssignmentDTO.File.FileName;
			}

			assignment.DueDate = updateAssignmentDTO.DueDate;
			assignment.DateUploaded = DateTime.Now; // Update the upload date

			try
			{
				// Save the updated assignment to the database
				_context.Assignments.Update(assignment);
				await _context.SaveChangesAsync();

				return Ok(new { message = "Assignment updated successfully.", assignment });
			}
			catch (Exception ex)
			{
				// Handle any errors that may occur
				return StatusCode(500, new { message = "An error occurred while updating the assignment.", error = ex.Message });
			}
		}


		//Delete assignment
		[HttpDelete("DeleteAssignmentBy{id}")]
		public async Task<IActionResult> DeleteAssignment(Guid id)
		{
			var assignment = await _context.Assignments.FindAsync(id);

			if (assignment == null)
			{
				return NotFound("Assignment Id not found");
			}

			_context.Assignments.Remove(assignment);
			await _context.SaveChangesAsync();

			return Ok("Successfully Deleted");
		}

	}

}

