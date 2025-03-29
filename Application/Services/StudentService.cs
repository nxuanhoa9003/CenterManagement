using Application.InterfacesServices;
using Domain.Entities;
using Domain.Interface;
using Infrastructures.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task AddStudent(Student student)
        {
            student.StudentId = Guid.NewGuid();
            student.CreatedAt = DateTime.UtcNow;
            await _studentRepository.AddStudentAsync(student);
        }

        public async Task UpdateStudent(Student student)
        {
            await _studentRepository.UpdateStudentAsync(student);
        }

        public async Task DeleteStudent(Guid Id)
        {
            var student = await _studentRepository.GetByIdAsync(Id);
            if (student != null)
            {
                await _studentRepository.DeleteStudentAsync(student);
            }
            else
            {
                throw new NotImplementedException("Not found student");
            }
        }

        public async Task<bool> CheckStudentExists(Guid? Id)
        {
            return await _studentRepository.StudentExists(Id);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetStudents();
        }

        public Task<Student?> GetStudentById(Guid? Id)
        {
            return _studentRepository.GetStudentById(Id);
        }

        public async Task<IEnumerable<Student>> GetStudentsByNames(string? name)
        {
          return await _studentRepository.GetStudentsByName(name);
        }

        public async Task<IEnumerable<Student>> GetStudentsByPhone(string? phone)
        {
            return await _studentRepository.GetStudentsByPhone(phone);
        }

        public async Task<Student?> GetStudentsByUserId(string? userid)
        {
            return await _studentRepository.GetStudentsByUserId(userid);
        }

    }
}
