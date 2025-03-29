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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddOrder(Order order)
        {
           await InsertAsync(order);
        }

        public async Task UpdateOrder(Order order)
        {
            await UpdateAsync(order);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid Id)
        {
           return await GetOneAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()=> await GetAllAsync();

        public async Task<IEnumerable<Order>> GetOrdersByStudentAsync(Guid Id)
        {
            return await GetManyAsync(x => x.StudentId ==  Id);
        }

        public async Task<bool> IsStudentPaid(Guid studentId, Guid CourseId)
        {
            var rc = await _dbSet.ToListAsync();
            Console.WriteLine(rc);
            return await AnyAsync(x => x.StudentId == studentId && x.CourseId == CourseId && x.Status == Domain.Enums.OrderStatus.Paid);
        }
    }
}
