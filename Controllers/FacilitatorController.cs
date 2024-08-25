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


		// PUT: api/Facilitator
		[HttpPut("UpdateFacilitatorBy{Id}")]
		public IActionResult UpdateStudentById(Facilitator facilitator)
		{
			var update = _facilitator.UpdateFacilitatorById(facilitator);
			if (update == null)
			{
				return NotFound();
			}
			return Ok(update);

		}

		//Get all facilitators
		
		[HttpGet]
		[Route("GetAllFacilitator")]
		public IActionResult GetAllFacilitators()
		{
			var facilitators = _facilitator.GetAllFacilitators();
			return Ok(facilitators);
		}

		//Get Facilitator By Id
		
		[HttpGet("GetFacilitatorBy{id}")]
		public async Task<ActionResult<Facilitator>> GetFacilitator(Guid id)
		{
			var facilitator = await _context.Facilitators.FindAsync(id);

			if (facilitator == null)
			{
				return NotFound();
			}

			return facilitator;
		}


		// POST: api/Facilitator
		[HttpPost]
		[Route("CreateFacilitator")]
		public IActionResult AddProfile(AddFacilitatorDTO facilitator)
		{
			_facilitator.AddFacilitator(facilitator);

			return Ok("Successful");
		}

		// DELETE: api/Facilitator/5
		
		[HttpDelete("DeleteFacilitatorBy{id}")]
		public async Task<IActionResult> DeleteFacilitator(Guid id)
		{
			var facilitator = await _context.Facilitators.FindAsync(id);
			if (facilitator == null)
			{
				return NotFound();
			}

			_context.Facilitators.Remove(facilitator);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		private bool FacilitatorExists(Guid id)
		{
			return _context.Facilitators.Any(e => e.FacilitatorId == id);
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

		//Create Schedule
		[HttpPost("CreateClassSchedule")]
		public async Task<ActionResult<ScheduleClass>> PostClassSchedule(ScheduleClass classSchedule)
		{
			classSchedule.Id = Guid.NewGuid();
			classSchedule.Date = classSchedule.Date;
			classSchedule.Duration = classSchedule.Duration;
			classSchedule.Time = classSchedule.Time;
			_context.ScheduleClasses.Add(classSchedule);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetClassSchedule), new { id = classSchedule.Id }, classSchedule);
		}

		// Get all scheduled classes
		[HttpGet("GetAllSchedule")]
		public async Task<ActionResult<IEnumerable<ScheduleClass>>> GetClassSchedules()
		{
			return await _context.ScheduleClasses.ToListAsync();
		}

		// Get a specific scheduled class by Id
		[HttpGet("GetParticularSchedule{id}")]
		public async Task<ActionResult<ScheduleClass>> GetClassSchedule(Guid id)
		{
			var classSchedule = await _context.ScheduleClasses.FindAsync(id);
			if (classSchedule == null)
			{
				return NotFound();
			}
			return classSchedule;
		}

		// Update an existing class schedule
		[HttpPut("UpdateSchedule{id}")]
		public async Task<IActionResult> PutClassSchedule(Guid id, ScheduleClass classSchedule)
		{
			if (id != classSchedule.Id)
			{
				return BadRequest();
			}

			_context.Entry(classSchedule).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ClassScheduleExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// Delete a class schedule
		[HttpDelete("DeleteScheduleBy{id}")]
		public async Task<IActionResult> DeleteClassSchedule(Guid id)
		{
			var classSchedule = await _context.ScheduleClasses.FindAsync(id);
			if (classSchedule == null)
			{
				return NotFound();
			}

			_context.ScheduleClasses.Remove(classSchedule);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ClassScheduleExists(Guid id)
		{
			return _context.ScheduleClasses.Any(e => e.Id == id);
		}

		//Upload Assignment
		[HttpPost("uploadAssignment")]
		public async Task<IActionResult> UploadAssignment(UploadAssignmentDTO uploadAssignmentDTO)
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
					FilePath = uploadAssignmentDTO.File.FileName,

				};

				_context.Assignments.Add(assignment);
				await _context.SaveChangesAsync();
			}

			return Ok("Content uploaded successfully.");
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

