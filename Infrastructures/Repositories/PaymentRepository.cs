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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Thêm thanh toán mới
        public async Task AddPaymentAsync(Payment payment) => await InsertAsync(payment);

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            return await GetAllAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStudent(Guid studentId)
        {
            return await GetManyAsync(x => x.StudentId == studentId);
        }
    }
}
