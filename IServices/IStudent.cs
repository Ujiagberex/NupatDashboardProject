using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface IStudent
    {
        void AddStudent(AddStudentDTO addStudentDTO);
        Student GetStudentById(Guid id);
		IEnumerable<Student> GetAllStudents();
		bool DeleteStudentById(Guid id);
        ShowStudentDTO UpdateStudentById(Student student);
		
	}
}
