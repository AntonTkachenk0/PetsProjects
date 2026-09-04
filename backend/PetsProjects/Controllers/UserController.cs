using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.Models;
using PetsProjects.Services;

namespace PetsProjects.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserController> _logger;
        private readonly UsersService _service;

        public UserController(IUserRepository repository, ILogger<UserController> logger, UsersService service)
        {
            _repository = repository;
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Authorize]
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


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> Update(
            Guid id, 
            [FromBody] string photoId,
            CancellationToken ct)
        {
            await _repository.UpdatePhoto(id, photoId,  ct);
            return Ok(id);
        }

        [HttpPut("update-info")]
        public async Task<ActionResult<Guid>> UpdateInfo([FromBody] UserUpdateRequest request, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Данные пользователя: {Request}", request);

                await _repository.UpdateInfo(request.Id, request.Name!, request.Login!, request.Email!, ct);

                return Ok(request.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, "Ошибка обновления данных. Данные {Request}", request);
                throw;
            }
        }
        

        [HttpGet("take-sum")]
        public async Task<ActionResult<decimal>> TakeSum([FromQuery] Guid userId,CancellationToken ct)
        {
            Console.WriteLine(userId);
           var result = await  _repository.TakeSum(userId, ct);

           return Ok(result);
        }

        [HttpGet("take-history")]
        public async Task<IActionResult> TakeHistory([FromQuery] Guid userId, CancellationToken ct)
        {
            _logger.LogInformation("{UserId}", userId);
            var result = await _repository.TakeHistory(userId, ct);
            
            return Ok(result);
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<Guid>> CreateUser(
            [FromBody] RegisterUserRequest request,
            CancellationToken ct)
        {
           await _service.Register(request.Name, request.Login, request.Email, request.Password, ct);

            // await _repository.Create(user.Value!, ct);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<ActionResult<Guid>> Login(
            [FromBody] LoginUserRequest request,
            CancellationToken ct)
        {
            var token = await _service.Login(request.Email, request.Password, ct);
            
            HttpContext.Response.Cookies.Append("tasty-cookies", token);
            
            return Ok(token);
        }
    }
}
