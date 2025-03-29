using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        Task<IEnumerable<Teacher>> GetTeachers();
        Task<IEnumerable<Teacher>> GetTeachersByName(string? name);
        Task<IEnumerable<Teacher>> GetTeachersByMail(string? mail);
        Task<IEnumerable<Teacher>> GetTeachersByPhone(string? phone);

        Task<Teacher?> GetTeacherById(Guid? Id);
        Task AddTeacherAsync(Teacher teacher);
        Task UpdateTeacherAsync(Teacher teacher);
        Task DeleteTeacherAsync(Teacher teacher);
        Task DeleteTeacherByIdAsync(Guid? id);
        Task<bool> TeacherExists(Guid? id);
    }
}
