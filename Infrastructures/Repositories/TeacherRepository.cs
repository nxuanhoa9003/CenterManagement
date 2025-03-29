using Domain.Entities;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace Infrastructures.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddTeacherAsync(Teacher teacher) => await InsertAsync(teacher);

        public async Task UpdateTeacherAsync(Teacher teacher) => await UpdateAsync(teacher);

        public async Task DeleteTeacherAsync(Teacher teacher) => await DeleteAsync(teacher);


        public async Task DeleteTeacherByIdAsync(Guid? id) => await DeleteAsync(x => x.TeacherId == id);

        public async Task<Teacher?> GetTeacherById(Guid? Id)
        {
            return await _dbSet.Include(x => x.User).FirstOrDefaultAsync(x => x.TeacherId == Id);
            //return await GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Teacher>> GetTeachers()
        {
            return await _dbSet.Include(x => x.User).ToListAsync();
            //return await GetAllAsync();
        }

        public async Task<IEnumerable<Teacher>> GetTeachersByMail(string? mail)
        {
            mail = mail ?? string.Empty;
            return await GetListAsync(
               filter: x => x.User.Email.ToLower().Contains(mail.ToLower()),
                null,
               includes: [c => c.User]
            );
        }

        public async Task<IEnumerable<Teacher>> GetTeachersByName(string? name)
        {
            name = name ?? string.Empty;
            return await GetListAsync(
               filter: x => x.User.FullName.ToUpper().Contains(name.ToUpper()),
                null,
               includes: [c => c.User]
            );
        }

        public async Task<IEnumerable<Teacher>> GetTeachersByPhone(string? phone)
        {
            phone = phone ?? string.Empty;
            return await GetListAsync(
               filter: x => x.User.PhoneNumber.ToUpper().Contains(phone.ToUpper()),
               includes: [c => c.User]
            );
        }

        public async Task<bool> TeacherExists(Guid? id) => await AnyAsync(x => x.TeacherId == id);


    }
}
