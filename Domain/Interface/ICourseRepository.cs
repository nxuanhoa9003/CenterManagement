using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetCourses();
        Task<IEnumerable<Course>> GetCoursesByNames(string? name);
        Task<Course?> GetCourseById(Guid Id);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(Guid Id);
        Task<bool> CourseExistsAsync(Guid Id);
    }
}
