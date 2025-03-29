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
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddLessonAsync(Lesson lesson) => await InsertAsync(lesson);

        public async Task UpdateLessonAsync(Lesson lesson) => await UpdateAsync(lesson);
        public async Task DeleteLessonAsync(Guid Id) => await DeleteAsync(x => x.Id == Id);

        public async Task<IEnumerable<Lesson>> GetAllLesson()
        {
            //return await GetAllAsync();
            return await GetListAsync(includes: [l => l.Class, l => l.Attendances]);
        }

        public async Task<IEnumerable<Lesson>> GetByNames(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetAllAsync();
            }

            return await GetManyAsync(x => x.Name != null &&  x.Name.Contains(name));
        }

        public async Task<Lesson?> GetLessonByIdAsync(Guid Id) => await GetByIdAsync(Id);

        public async Task<bool> LessonExistsAsync(Guid Id) => await AnyAsync(x => x.Id == Id);

        
    }
}
