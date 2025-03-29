using Domain.Entities;
using Domain.Enums;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructures.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddClassAsync(Class entity) => await InsertAsync(entity);

        public async Task UpdateClassAsync(Class entity) => await UpdateAsync(entity);


        public async Task DeleteClassAsync(Guid Id) => await DeleteAsync(x => x.Id == Id);

        public async Task<Class?> FindClassByIdAsync(Guid? Id) => await GetByIdAsync(Id);
        public async Task<IEnumerable<Class>> FindClassByName(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetAllAsync();
            }

            return await GetManyAsync(x => x.Name != null && x.Name.Contains(name));
        }

        public async Task<IEnumerable<Class>> FindClassByTeacher(Guid? TeacherId)
        {
            return await GetManyAsync(x => x.TeacherId != Guid.Empty && x.TeacherId == TeacherId);
        }

        public async Task<IEnumerable<Class>> GetClasses()
        {
            return await GetListAsync(includes: [c => c.Course, c => c.Teacher ,c => c.Lessons, c => c.Enrollments]);
        }

        public async Task<IEnumerable<Class>> GetClassesByStatus(ClassStatus? status)
        {
            var query = _dbSet.AsQueryable();
            var now = DateTime.Now;
            if (status.HasValue)
            {
                switch (status.Value)
                {
                    case ClassStatus.Ongoing:
                        query = query.Where(c => c.StartDate <= now && c.EndDate >= now);
                        break;
                    case ClassStatus.Upcoming:
                        query = query.Where(c => c.StartDate > now);
                        break;
                    case ClassStatus.Completed:
                        query = query.Where(c => c.EndDate < now);
                        break;
                }
            }

            return await query.ToListAsync();
        }
        public async Task<bool> ClassExistsAsync(Guid Id) => await AnyAsync(x => x.Id == Id);
        public async Task<bool> ClassNameExistsAsync(string name) => await AnyAsync(x => x.Name.ToUpper() == name.ToUpper());


    }
}
