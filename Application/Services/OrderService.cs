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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task AddOrder(Order order)
        {
            await _orderRepository.AddOrder(order);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid Id)
        {
            return await _orderRepository.GetOrderByIdAsync(Id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStudentAsync(Guid Id)
        {
            return await _orderRepository.GetOrdersByStudentAsync(Id);
        }

        public async Task<bool> IsStudentPaid(Guid StudentId, Guid CourseId)
        {
            return await _orderRepository.IsStudentPaid(StudentId, CourseId);
        }

        public async Task UpdateOrder(Order order)
        {
            await _orderRepository.UpdateOrder(order);
        }
    }
}
