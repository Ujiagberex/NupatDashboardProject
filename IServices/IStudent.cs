using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface IStudent
    {
		Task<string> GetStudentById(string id);
		IEnumerable<Student> GetAllStudents();
		bool DeleteStudentById(Guid id);

	}
}
