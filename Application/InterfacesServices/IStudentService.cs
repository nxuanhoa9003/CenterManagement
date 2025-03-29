using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();

        Task<IEnumerable<Student>> GetStudentsByNames(string? name);

        Task<IEnumerable<Student>> GetStudentsByPhone(string? phone);

        Task<Student?> GetStudentById(Guid? Id);

        Task<Student?> GetStudentsByUserId(string? userid);

        Task AddStudent(Student student);

        Task UpdateStudent(Student student);

        Task DeleteStudent(Guid Id);

        Task<bool> CheckStudentExists(Guid? Id);
    }
}
