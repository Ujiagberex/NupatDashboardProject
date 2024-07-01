using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
    public class StudentService : IStudent
	{
			private readonly LmsDbContext _dataContext;
			public StudentService(LmsDbContext dataContext)
			{
				_dataContext = dataContext;
			}
		public void AddStudent(AddStudentDTO addStudentDTO)
		{
			
		Student student = new Student
			{
				StudentId = Guid.NewGuid(),
				FullName = addStudentDTO.FullName,
				CourseId = addStudentDTO.CourseId,
				CohortId = addStudentDTO.CohortId
			};

				_dataContext.Students.Add(student);
				_dataContext.SaveChanges();
				
			}

		public bool DeleteStudentById(Guid id)
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

		public Student GetStudentById(Guid id)
			{
				return _dataContext.Students.Find(id);
			}

		public ShowStudentDTO UpdateStudentById(Student student)
			{
			var FindStudent = GetStudentById(student.StudentId);
			if (FindStudent == null)
			{
				return null;

			}
			FindStudent.FullName = student.FullName;
			_dataContext.SaveChanges();
			ShowStudentDTO showStudentDTO = new ShowStudentDTO();
			showStudentDTO.FullName = student.FullName;
			return showStudentDTO;
			}
	}
}
