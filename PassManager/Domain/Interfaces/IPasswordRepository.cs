using PassManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Domain.Interfaces
{
    public interface IPasswordRepository
    {
        Task<IEnumerable<PasswordItem>> GetAllAsync();
        Task<PasswordItem> GetByIdAsync(int id);
        Task AddAsync(PasswordItem item);
        Task UpdateAsync(PasswordItem item);
        Task DeleteAsync(int id);
    }
}
