using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using testing.Core.Domain.Entities;

namespace testing.Core.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
