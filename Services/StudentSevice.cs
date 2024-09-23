using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;
using ServiceStack;

namespace NupatDashboardProject.Services
{
	public class StudentService : IStudent
	{
		private readonly LmsDbContext _dataContext;
		private readonly UserManager<ApplicationUser> _userManager;
		public StudentService(LmsDbContext dataContext, UserManager<ApplicationUser> userManager)
		{
			_dataContext = dataContext;
			_userManager = userManager;
		}
		public async Task<string> GetStudentById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return ("Student ID is required");
			}

			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return ("Student not found");
			}

            // Cast to Student
            var student = user as Student;

            if (student == null)
            {
                return ("User is not a student");
            }

            var studentDTO = new StudentDTO()
			{
				Id = student.Id,
				Email = student.Email,
				FullName = student.FullName,
				CohortId = student.CohortId,
				CourseId = student.CourseId
			};

			return studentDTO.ToString();

		}

		public bool DeleteStudentById(string id)
			{
				Student student = _dataContext.Students.Find(id);
				if (student == null)
				{
					return false;
				}
				_dataContext.Remove(student);
				_dataContext.SaveChanges();

				return true;
			}

		public IEnumerable<Student> GetAllStudents()
			{
			return _dataContext.Students.AsEnumerable();
			}

		
	}
}
