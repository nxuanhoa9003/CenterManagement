using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCourses();

        Task<IEnumerable<Course>> GetCoursesByNames(string? name);

        Task<Course?> GetCourseById(Guid Id);

        Task AddCourse(Course course);

        Task UpdateCourse(Course course);

        Task DeleteCourse(Guid Id);

        Task<bool> IsCourseExists(Guid Id);

    }
}
