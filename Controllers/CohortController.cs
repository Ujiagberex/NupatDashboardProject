using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CohortController : ControllerBase
	{
		private readonly LmsDbContext _context;

        public CohortController(LmsDbContext context)
        {
			_context = context; 
        }

		[HttpGet("GetAllCohorts")]
		public async Task<ActionResult<IEnumerable<Cohort>>> GetCohorts()
		{
			return await _context.Cohorts.ToListAsync();
		}

		// GET: api/Cohort/5
		[HttpGet("GetCohortBy{id}")]
		public async Task<ActionResult<Cohort>> GetCohort(Guid id)
		{
			var cohort = await _context.Cohorts.FirstOrDefaultAsync(c => c.CohortId == id);

			if (cohort == null)
			{
				return NotFound();
			}

			return cohort;
		}

		// PUT: api/Cohort/5
		[HttpPut("UpdateCohort{id}")]
		public async Task<IActionResult> PutCohort(Guid id, Cohort cohort)
		{
			if (id != cohort.CohortId)
			{
				return BadRequest();
			}

			_context.Entry(cohort).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CohortExists(id))
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


		// POST: api/Cohort
		[HttpPost("CreateCohort")]
		public async Task<ActionResult<Cohort>> PostCohort(Cohort cohort)
		{
			_context.Cohorts.Add(cohort);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCohort", new { id = cohort.CohortId }, cohort);
		}

		// DELETE: api/Cohort/5
		[HttpDelete("DeleteCohortBy{id}")]
		public async Task<IActionResult> DeleteCohort(Guid id)
		{
			var cohort = await _context.Cohorts.FindAsync(id);
			if (cohort == null)
			{
				return NotFound();
			}

			_context.Cohorts.Remove(cohort);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CohortExists(Guid id)
		{
			return _context.Cohorts.Any(e => e.CohortId == id);
		}
	}
}
