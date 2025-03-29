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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task AddPaymentAsync(Payment payment)
        {
           await _paymentRepository.AddPaymentAsync(payment);
        }

        public Task<IEnumerable<Payment>> GetPayments()
        {
            return _paymentRepository.GetPayments();
        }

        public Task<IEnumerable<Payment>> GetPaymentsByStudent(Guid studentId)
        {
            return _paymentRepository.GetPaymentsByStudent(studentId);
        }
    }
}
