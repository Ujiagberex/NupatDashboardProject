using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
    public interface ICourse
    {
        void AddCourse(AddCourseDTO addCourseDTO);
        Course GetcourseById(Guid id);
        IEnumerable<Course> GetAllCourses();
        bool DeleteCourseById(Guid id);
        ShowStudentDTO UpdateById(Course course);
    }
}
