using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task AddToken(RefreshToken token);
        Task UpdateToken(RefreshToken token);
        Task<RefreshToken?> GetToken(string token);
        Task DeleteTokeByUserId(string userId);
    }
}
