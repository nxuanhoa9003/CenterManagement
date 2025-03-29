using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetClasses();

        Task<IEnumerable<Class>> FindClassByName(string? name);

        Task<IEnumerable<Class>> GetClassesByStatus(ClassStatus? status);

        Task<IEnumerable<Class>> FindClassByTeacher(Guid teacherId);

        Task<Class?> FindClassById(Guid Id);
        Task AddClass(Class newClass);

        Task UpdateClass(Class newClass);

        Task DeleteClass(Guid Id);

        Task<IEnumerable<Teacher?>> GetTeachersInClass();
        Task<int> CountStudentInClass(Guid classId);
        Task<IEnumerable<Student?>> GetStudentsInClass(Guid classId);
        Task<bool> CheckStudentInClass(Guid classId, Guid studentId);
        Task DeleteStudentFromClass(Guid classId, Guid studentId);
        Task<bool> CheckClassIdExist(Guid classId);
        Task<bool> ClassNameExistsAsync(string name);
    }
}
