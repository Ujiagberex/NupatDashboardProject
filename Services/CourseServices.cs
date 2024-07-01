using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
	public class CourseServices : ICourse
	{
		private readonly LmsDbContext _dbContext;
        public CourseServices(LmsDbContext dbContext)
        {
			_dbContext = dbContext;	
        }
        public void AddCourse(AddCourseDTO addCourseDTO)
		{
			Course course = new Course
			{
				CourseId = Guid.NewGuid(),
				Title = addCourseDTO.Title,
				FacilitatorId = addCourseDTO.FacilitatorId
			};

			_dbContext.Courses.Add(course);
			_dbContext.SaveChanges();
		}

		public bool DeleteCourseById(Guid id)
		{
			Course course = _dbContext.Courses.Find(id);
			if (course == null)
			{
				return false;
			}
			_dbContext.Remove(course);
			_dbContext.SaveChanges();

			return true;
		}

		public IEnumerable<Course> GetAllCourses()
		{
			throw new NotImplementedException();
		}

		public Course GetcourseById(Guid id)
		{
			throw new NotImplementedException();
		}

		public ShowStudentDTO UpdateById(Course course)
		{
			throw new NotImplementedException();
		}
	}
}
