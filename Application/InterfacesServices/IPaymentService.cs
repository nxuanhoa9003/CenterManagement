using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IPaymentService
    {
        Task AddPaymentAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPayments();
        Task<IEnumerable<Payment>> GetPaymentsByStudent(Guid studentId);
    }
}
