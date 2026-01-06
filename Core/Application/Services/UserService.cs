using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testing.Core.Application.DTOs;
using testing.Core.Domain.Entities;
using testing.Core.Domain.Interfaces;

namespace testing.Core.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task<UserReadDto?> GetUserByIdAsync(Guid id);
        Task<UserReadDto> CreateUserAsync(UserCreateDto userDto);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMessageBusClient _messageBus;

        public UserService(IUserRepository repository, IMessageBusClient messageBus)
        {
            _repository = repository;
            _messageBus = messageBus;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(u => new UserReadDto 
            { 
                Id = u.Id, 
                Name = u.Name, 
                Email = u.Email, 
                CreatedAt = u.CreatedAt 
            });
        }

        public async Task<UserReadDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserReadDto 
            { 
                Id = user.Id, 
                Name = user.Name, 
                Email = user.Email, 
                CreatedAt = user.CreatedAt 
            };
        }

        public async Task<UserReadDto> CreateUserAsync(UserCreateDto userDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userDto.Name,
                Email = userDto.Email,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateAsync(user);

            // Publicar evento en RabbitMQ
            try 
            {
                _messageBus.PublishUserCreated($"User Created: {user.Name} ({user.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return new UserReadDto 
            { 
                Id = user.Id, 
                Name = user.Name, 
                Email = user.Email, 
                CreatedAt = user.CreatedAt 
            };
        }
    }
}
