using Application.InterfacesServices;
using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task AddCourse(Course course)
        {
            await _courseRepository.AddCourseAsync(course);
        }

        public async Task UpdateCourse(Course course)
        {
            await _courseRepository.UpdateCourseAsync(course);
        }

        public async Task DeleteCourse(Guid Id)
        {
            await _courseRepository.DeleteCourseAsync(Id);
        }

        public async Task<Course?> GetCourseById(Guid Id)
        {
            return await _courseRepository.GetCourseById(Id);
        }

        public Task<IEnumerable<Course>> GetCourses()
        {
            return _courseRepository.GetCourses();
        }

        public async Task<IEnumerable<Course>> GetCoursesByNames(string? name)
        {
            return await _courseRepository.GetCoursesByNames(name);
        }

        public async Task<bool> IsCourseExists(Guid Id)
        {
            return await _courseRepository.CourseExistsAsync(Id);
        }

       
    }
}
