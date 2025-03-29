using Application.InterfacesServices;
using Domain.Entities;
using Domain.Enums;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;

        public ClassService(IClassRepository classRepository, IEnrollmentRepository enrollmentRepository, IStudentRepository studentRepository, ITeacherRepository teacherRepository)
        {
            _classRepository = classRepository;
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task AddClass(Class newClass)
        {
            await _classRepository.InsertAsync(newClass);
        }

        public async Task UpdateClass(Class newClass)
        {
            await _classRepository.UpdateClassAsync(newClass);
        }

        public async Task DeleteClass(Guid Id)
        {
            await _classRepository.DeleteClassAsync(Id);
        }

        public Task<bool> CheckClassIdExist(Guid classId)
        {
            return _classRepository.ClassExistsAsync(classId);
        }

        public async Task<bool> CheckStudentInClass(Guid classId, Guid studentId)
        {
            return await _enrollmentRepository.IsStudentEnrolled(studentId, classId);
        }

        public async Task<int> CountStudentInClass(Guid classId)
        {
            return await _enrollmentRepository.CountStudentInClass(classId);
        }

        public async Task DeleteStudentFromClass(Guid classId, Guid studentId)
        {
            await _enrollmentRepository.DeleteEnrollmentAsync(studentId, classId);
        }

        public async Task<Class?> FindClassById(Guid Id)
        {
            return await _classRepository.FindClassByIdAsync(Id);
        }

        public async Task<IEnumerable<Class>> FindClassByName(string? name)
        {
            return await _classRepository.FindClassByName(name);
        }

        public async Task<IEnumerable<Class>> FindClassByTeacher(Guid teacherId)
        {
            return await _classRepository.FindClassByTeacher(teacherId);
        }

        public async Task<IEnumerable<Class>> GetClasses()
        {
            return await _classRepository.GetClasses();
        }

        public async Task<IEnumerable<Class>> GetClassesByStatus(ClassStatus? status)
        {

            return await _classRepository.GetClassesByStatus(status);
        }

        public async Task<IEnumerable<Student?>> GetStudentsInClass(Guid classId)
        {
            var erroll = await _enrollmentRepository.GetEnrollmentsByClassId(classId);
            var listStudent = erroll.Select(x => x.Student).ToList();
            return listStudent;
        }

        public async Task<IEnumerable<Teacher?>> GetTeachersInClass()
        {
            return await _teacherRepository.GetTeachers();
        }

        public async Task<bool> ClassNameExistsAsync(string name)
        {
            return await _classRepository.ClassNameExistsAsync(name);
        }
    }
}
