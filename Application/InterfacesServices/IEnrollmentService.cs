using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IEnrollmentService
    {
        Task AddEnrollmentAsync(Enrollment model);
        Task RemoveEnrollmentAsync(Enrollment model);
        Task<Enrollment?> GetEnrollmentAsync(Guid studentId, Guid classId);
        Task<IEnumerable<Enrollment>> GetStudentsByClassIdAsync(Guid classId);
        Task<IEnumerable<Enrollment>> GetClassesByStudentIdAsync(Guid studentId);
        Task<bool> IsStudentEnrolled(Guid studentId, Guid classId);
        Task<int> CountClassBuyStudentAsync(Guid studentId);
    }
}
