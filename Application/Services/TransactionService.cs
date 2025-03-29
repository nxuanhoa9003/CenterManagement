using Application.InterfacesServices;
using Infrastructures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteTransactionAsync(Func<Task> action)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action(); // Thực thi hàm truyền vào

                await _context.SaveChangesAsync(); // Lưu dữ liệu nếu thành công
                await transaction.CommitAsync(); // Commit transaction
            }
            catch
            {
                await transaction.RollbackAsync(); // Rollback nếu có lỗi
                throw; // Ném lỗi ra ngoài để service xử lý
            }
        }

        public async Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                T result = await action(); // Thực thi hàm truyền vào và lấy kết quả

                await _context.SaveChangesAsync(); // Lưu dữ liệu nếu thành công
                await transaction.CommitAsync(); // Commit transaction

                return result; // Trả về kết quả
            }
            catch
            {
                await transaction.RollbackAsync(); // Rollback nếu có lỗi
                throw; // Ném lỗi ra ngoài để service xử lý
            }
        }
    }
}
