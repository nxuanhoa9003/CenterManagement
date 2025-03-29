using Application.InterfacesServices;
using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task AddTeacherAsync(Teacher teacher)
        {
            teacher.TeacherId = Guid.NewGuid();
            teacher.CreatedAt = DateTime.UtcNow;
            await _teacherRepository.AddTeacherAsync(teacher);
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            await _teacherRepository.UpdateTeacherAsync(teacher);
        }

        public async Task DeleteTeacherByIdAsync(Guid id)
        {
            await _teacherRepository.DeleteTeacherByIdAsync(id);
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _teacherRepository.GetTeachers();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(Guid id)
        {
            return await _teacherRepository.GetTeacherById(id);
        }

        public Task<bool> IsTeacherExist(Guid Id)
        {
            return _teacherRepository.TeacherExists(Id);
        }
 
    }
}
