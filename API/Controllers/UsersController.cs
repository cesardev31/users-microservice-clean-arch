using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using testing.Core.Application.DTOs;
using testing.Core.Application.Services;

namespace testing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserCreateDto> _validator;

        public UsersController(IUserService userService, IValidator<UserCreateDto> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [HttpGet("health")]
        [EndpointSummary("Verificar salud del servicio")]
        [EndpointDescription("Retorna el estado de conectividad del microservicio de usuarios.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Health() => Ok(new { status = "healthy" });

        [HttpGet("status")]
        [EndpointSummary("Obtener estado operativo")]
        [EndpointDescription("Proporciona información sobre la versión y el tiempo de actividad del servicio.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Status() => Ok(new { service = "users", uptime = "active" });

        [HttpGet]
        [EndpointSummary("Listar todos los usuarios")]
        [EndpointDescription("Recupera una lista completa de todos los usuarios registrados en el sistema desde MongoDB.")]
        [ProducesResponseType(typeof(IEnumerable<UserReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [EndpointSummary("Obtener usuario por ID")]
        [EndpointDescription("Busca un usuario específico utilizando su identificador único (GUID).")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserReadDto>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPost]
        [EndpointSummary("Crear nuevo usuario")]
        [EndpointDescription("Registra un nuevo usuario, lo guarda en la base de datos y publica un evento en RabbitMQ.")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var validationResult = await _validator.ValidateAsync(userCreateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userReadDto = await _userService.CreateUserAsync(userCreateDto);

            return CreatedAtRoute(nameof(GetUserById), new { Id = userReadDto.Id }, userReadDto);
        }
    }
}
