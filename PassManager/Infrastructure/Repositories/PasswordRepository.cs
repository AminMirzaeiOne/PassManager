using PassManager.Domain.Entities;
using PassManager.Domain.Interfaces;
using PassManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Infrastructure.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly AppDBContext _ctx;
        public PasswordRepository(AppDBContext ctx) => _ctx = ctx;

        public async Task AddAsync(PasswordItem item)
        {
            _ctx.PasswordItems.Add(item);
            await _ctx.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var e = await _ctx.PasswordItems.FindAsync(id);
            if (e != null) { _ctx.PasswordItems.Remove(e); await _ctx.SaveChangesAsync(); }
        }
        public async Task<IEnumerable<PasswordItem>> GetAllAsync()
            => await _ctx.PasswordItems.AsNoTracking().ToListAsync();

        public async Task<PasswordItem> GetByIdAsync(int id)
            => await _ctx.PasswordItems.FindAsync(id);

        public async Task UpdateAsync(PasswordItem item)
        {
            _ctx.PasswordItems.Update(item);
            await _ctx.SaveChangesAsync();
        }
    }
}
