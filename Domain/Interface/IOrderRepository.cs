using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
  public  interface IOrderRepository
    {
        Task<Order?>GetOrderByIdAsync(Guid Id);
        Task<IEnumerable<Order>>GetOrdersAsync();
        Task<IEnumerable<Order>>GetOrdersByStudentAsync(Guid Id);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);

        Task<bool> IsStudentPaid(Guid studentId, Guid CourseId);
        
    }
}
