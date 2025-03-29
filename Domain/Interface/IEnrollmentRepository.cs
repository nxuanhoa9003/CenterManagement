using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task DeleteEnrollmentAsync(Guid studentId, Guid classId);
        Task<Enrollment?> GetEnrollmentAsync(Guid studentId, Guid classId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentId(Guid studentId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByClassId(Guid classId);
        Task<bool> IsStudentEnrolled(Guid studentId, Guid classId);
        Task<int> CountStudentInClass(Guid classId);
        Task<int> CountClassBuyStudent(Guid studentId);
    }
}
