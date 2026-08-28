using Microsoft.AspNetCore.Mvc;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.Models;

namespace PetsProjects.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository repository, ILogger<UserController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> Get(CancellationToken ct)
        {
            var user = await _repository.Get(ct);

            var response = user.Select(p => new UserResponse(p.Id, p.Name, p.Login, p.Email, p.PasswordHached, p.PhotoS3Key));

            return Ok(response);
        }
        
        [HttpGet("user")]  
        public async Task<ActionResult<UserResponse>> GetById(
            [FromQuery] Guid id,  
            CancellationToken ct)
        {
            var user = await _repository.GetById(id, ct);
    
            if (user == null)
                return NotFound($"Пользователь не найден");

            var response = new UserResponse(
                user.Id, 
                user.Name, 
                user.Login, 
                user.Email, 
                user.PasswordHached, 
                user.PhotoS3Key
            );

            return Ok(response);
        }
        
        [HttpGet("minimalInfo")]
        public async Task<ActionResult<List<UserResponse>>> GetMinimal(CancellationToken ct)
        {
            var user = await _repository.Get(ct);

            var response = user.Select(p => new
            {
                p.Id,
                p.Name
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser(
            [FromBody] UserRequest request,
            CancellationToken ct)
        {
            var user = UserModel.Create(
                Guid.NewGuid(),
                request.Name,
                request.Login,
                request.Email,
                request.PasswordHached,
                "default");

            if (!user.IsSuccess)
            {
                Console.WriteLine("Ошибка");
                return BadRequest(user);
            }

            await _repository.Create(user.Value!, ct);
            return Ok(user);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> Update(
            Guid id, 
            [FromBody] string photoId,
            CancellationToken ct)
        {
            await _repository.UpdatePhoto(id, photoId,  ct);
            return Ok(id);
        }

        [HttpGet("take-sum")]
        public async Task<ActionResult<decimal>> TakeSum([FromQuery] Guid id_user,CancellationToken ct)
        {
            Console.WriteLine(id_user);
           var result = await  _repository.TakeSum(id_user, ct);

           return Ok(result);
        }

        [HttpGet("take-history")]
        public async Task<IActionResult> TakeHistory([FromQuery] Guid id_user, CancellationToken ct)
        {
            _logger.LogInformation($"{id_user}");
            var result = await _repository.TakeHistory(id_user, ct);
            
            return Ok(result);
        }
    }
}
