using Domain.Entities;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddCourseAsync(Course course) => await InsertAsync(course);

        public async Task UpdateCourseAsync(Course course) => await UpdateAsync(course);


        public async Task DeleteCourseAsync(Guid Id) => await DeleteAsync(x => x.Id == Id);
        public async Task<Course?> GetCourseById(Guid Id) => await GetByIdAsync(Id);

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await GetListAsync(
                null,
                null,
                c => c.Category,
                c => c.Classes
            );
        }

        public async Task<IEnumerable<Course>> GetCoursesByNames(string? name)
        {

            name = name ?? string.Empty;
            return await GetListAsync(
                e => e.CourseName.Contains(name),
                null,
                c => c.Category,
                c => c.Classes
            );
        }


        public async Task<bool> CourseExistsAsync(Guid Id) => await AnyAsync(e => e.Id == Id);
    }
}
