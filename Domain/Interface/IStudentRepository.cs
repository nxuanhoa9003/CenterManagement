using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudents();
        Task<IEnumerable<Student>> GetStudentsByName(string? name);
        Task<IEnumerable<Student>> GetStudentsByMail(string? mail);
        Task<IEnumerable<Student>> GetStudentsByPhone(string? phone);
        Task<Student?> GetStudentsByUserId(string? userId);

        Task<Student?> GetStudentById(Guid? Id);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(Student student);
        Task DeleteStudentByIdAsync(Guid? id);
        Task<bool> StudentExists(Guid? id);

    }
}
