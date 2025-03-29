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
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddStudentAsync(Student student) => await InsertAsync(student);

        public async Task UpdateStudentAsync(Student student) => await UpdateAsync(student);

        public async Task DeleteStudentAsync(Student student) => await DeleteAsync(student);
        public async Task DeleteStudentByIdAsync(Guid? id) => await DeleteAsync(x => x.StudentId == id);
        public async Task<Student?> GetStudentById(Guid? Id) => await GetByIdAsync(Id);

        public async Task<IEnumerable<Student>> GetStudents() => await GetAllAsync();

        public async Task<IEnumerable<Student>> GetStudentsByMail(string? mail)
        {
            mail = mail ?? string.Empty;
            return await GetListAsync(
               filter: x => x.User.Email.ToLower().Contains(mail.ToLower()),
                null,
               includes: [c => c.User]
            );
        }

        public async Task<IEnumerable<Student>> GetStudentsByName(string? name)
        {
            name = name ?? string.Empty;
            return await GetListAsync(
               filter: x => x.User.FullName.ToUpper().Contains(name.ToUpper()),
                null,
               includes: [c => c.User]
            );

        }

        public async Task<IEnumerable<Student>> GetStudentsByPhone(string? phone)
        {
            phone = phone ?? string.Empty;
            return await GetListAsync(
               filter: x => x.User.PhoneNumber.ToUpper().Contains(phone.ToUpper()),
                null,
               includes: [c => c.User]
            );
        }

        public async Task<bool> StudentExists(Guid? id) => await AnyAsync(x => x.StudentId == id);

        public async Task<Student?> GetStudentsByUserId(string? userId)
        {
            return await GetOneAsync(x => x.UserId == userId);
        }
    }
}
