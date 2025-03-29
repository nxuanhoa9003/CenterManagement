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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task AddEnrollmentAsync(Enrollment model)
        {
            await _enrollmentRepository.AddEnrollmentAsync(model);
        }

        public async Task RemoveEnrollmentAsync(Enrollment model)
        {
            await _enrollmentRepository.DeleteEnrollmentAsync(model.StudentId, model.ClassId);
        }

        public async Task<IEnumerable<Enrollment>> GetClassesByStudentIdAsync(Guid studentId)
        {
            return await _enrollmentRepository.GetEnrollmentsByStudentId(studentId);
        }

        public async Task<IEnumerable<Enrollment>> GetStudentsByClassIdAsync(Guid classId)
        {
            return await _enrollmentRepository.GetEnrollmentsByClassId(classId);
        }

        public async Task<bool> IsStudentEnrolled(Guid studentId, Guid classId)
        {
            return await _enrollmentRepository.IsStudentEnrolled(studentId, classId);
        }

        public async Task<Enrollment?> GetEnrollmentAsync(Guid studentId, Guid classId)
        {
            return await _enrollmentRepository.GetEnrollmentAsync(studentId, classId);
        }

        public async Task<int> CountClassBuyStudentAsync(Guid studentId)
        {
            return await _enrollmentRepository.CountClassBuyStudent(studentId);
        }
    }
}
