using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
   public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task AddPaymentAsync(Payment payment);
        Task <IEnumerable<Payment>>GetPayments();
        Task <IEnumerable<Payment>>GetPaymentsByStudent(Guid studentId);

    }
}
