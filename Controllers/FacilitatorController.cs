using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

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

		[Authorize]
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
		[Authorize]
		[HttpGet]
		[Route("GetAllFacilitator")]
		public IActionResult GetAllFacilitators()
		{
			var facilitators = _facilitator.GetAllFacilitators();
			return Ok(facilitators);
		}

		//Get Facilitator By Id
		[Authorize]
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
		[Authorize]
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
					CohortId = uploadContentDTO.CohortId,
					FileName = uploadContentDTO.File.FileName,
					FileData = memoryStream.ToArray(),
					UploadDate = DateTime.Now
				};

				_context.Contents.Add(content);
				await _context.SaveChangesAsync();
			}

			return Ok("Content uploaded successfully.");

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

			return NoContent();
		}
		//Addtest
		[HttpPost("AddTest")]
		public async Task<ActionResult<Test>> AddTest(Test newTest)
		{
			if (newTest == null)
			{
				return BadRequest("Test cannot be null.");
			}

			newTest.TestId = Guid.NewGuid(); // Ensure the TestId is set to a new GUID
			_context.Tests.Add(newTest);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetTestById), new { id = newTest.TestId }, newTest);
		}
		//Delete Test By Id
		[HttpDelete("DeleteTestBy{id}")]
		public async Task<IActionResult> DeleteTest(Guid id)
		{
			var test = await _context.Tests.FindAsync(id);

			if (test == null)
			{
				return NotFound();
			}

			_context.Tests.Remove(test);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		//Get test by Id
		[HttpGet("GetTestBy{id}")]
		public async Task<ActionResult<Test>> GetTestById(Guid id)
		{
			var test = await _context.Tests.FindAsync(id);

			if (test == null)
			{
				return NotFound();
			}

			return Ok(test);
		}

		//Create Schedule
		[HttpPost("CreateClassSchedule")]
		public async Task<ActionResult<ScheduleClass>> PostClassSchedule(ScheduleClass classSchedule)
		{
			classSchedule.Id = Guid.NewGuid();
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
	}

}

