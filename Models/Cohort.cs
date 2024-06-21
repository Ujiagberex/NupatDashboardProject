using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NupatDashboardProject.Models
{
	public class Cohort
	{
		[Key]
		public Guid CohortId { get; set; }
		public string Name { get; set; }
	}
}
