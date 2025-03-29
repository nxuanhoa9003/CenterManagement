using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface ITransactionService
    {
        Task ExecuteTransactionAsync(Func<Task> action);
        Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action);
    }
}
