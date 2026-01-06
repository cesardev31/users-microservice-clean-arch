using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using testing.Core.Domain.Entities;
using testing.Core.Domain.Interfaces;

namespace testing.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.GetCollection<User>("Users");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _users.DeleteOneAsync(u => u.Id == id);
        }
    }
}
