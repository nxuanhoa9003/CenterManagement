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
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task AddToken(RefreshToken token) => await InsertAsync(token);
        public async Task<RefreshToken?> GetToken(string token) => await GetOneAsync(x => x.Token == token);
        public async Task UpdateToken(RefreshToken token) => await UpdateAsync(token);
        public async Task DeleteTokeByUserId(string userId) => await DeleteAsync(x => x.UserId == userId);
    }

}
