using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<IEnumerable<Class>> GetClasses();
        Task<IEnumerable<Class>> FindClassByName(string? name);
        Task<IEnumerable<Class>> GetClassesByStatus(ClassStatus? status);
        Task<IEnumerable<Class>> FindClassByTeacher(Guid? TeacherId);
        Task<Class?> FindClassByIdAsync(Guid? Id);
        Task AddClassAsync(Class entity);
        Task UpdateClassAsync(Class entity);
        Task DeleteClassAsync(Guid Id);
        Task<bool> ClassExistsAsync(Guid Id);
        Task<bool> ClassNameExistsAsync(string name);


    }
}
