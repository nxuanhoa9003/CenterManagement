using Domain.Entities;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
        }
        // Thêm đăng ký lớp học
        public async Task AddEnrollmentAsync(Enrollment enrollment) => await InsertAsync(enrollment);

        // Cập nhật trạng thái đăng ký
        public async Task UpdateEnrollmentAsync(Enrollment enrollment) => await UpdateAsync(enrollment);


        // Xóa đăng ký lớp học
        public async Task DeleteEnrollmentAsync(Guid studentId, Guid classId) => await DeleteAsync(x => x.StudentId == studentId && x.ClassId == x.ClassId);


        public async Task<Enrollment?> GetEnrollmentAsync(Guid studentId, Guid classId) => await GetOneAsync(x => x.StudentId == studentId && x.ClassId == x.ClassId);

        // Lấy danh sách học viên trong một lớp
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByClassId(Guid classId)
        {

            return await GetListAsync(
               filter: x => x.ClassId == classId,
                null,
               includes: [c => c.Student]
            );

        }
        // Lấy danh sách lớp của một học viên
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentId(Guid studentId)
        {
            return await GetListAsync(
               filter: x => x.StudentId == studentId,
                null,
               includes: [c => c.Class]
            );
        }

        // Kiểm tra học viên có đăng ký lớp hay không
        public async Task<bool> IsStudentEnrolled(Guid studentId, Guid classId)
        {
            return await AnyAsync(x => x.StudentId == studentId && x.ClassId == classId);
        }

        public async Task<int> CountStudentInClass(Guid classId)
        {
            return await CountAsync(x => x.ClassId == classId);
        }

        public async Task<int> CountClassBuyStudent(Guid studentId)
        {
            return await CountAsync(x => x.StudentId == studentId);
        }
    }
}
