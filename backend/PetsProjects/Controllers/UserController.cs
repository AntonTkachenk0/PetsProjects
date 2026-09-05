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
        [Authorize("AdminPolicy")]
        public async Task<ActionResult<List<UserResponse>>> Get(CancellationToken ct)
        {
            
            var userId = User.FindFirst("Admin")?.Value;
            
            _logger.LogInformation("Полученный токен {UserId}", userId);
            var user = await _repository.Get(ct);

            var response = user.Select(p => new UserResponse(p.Id, p.Name, p.Login, p.Email, p.PasswordHached, p.PhotoS3Key));

            return Ok(response);
        }
        
        [HttpGet("user")]  
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetById(CancellationToken ct)
        {
            var userId = User.FindFirst("userId")?.Value;


            if (userId == null)
            {
                _logger.LogError("Равен Id null");
                return BadRequest("Ошибка пользователя. Id равен null");
            }
            _logger.LogInformation("Попытка запарсить id {UserId}", userId);
                
            var userIdGuid =  Guid.Parse(userId);
                
            _logger.LogInformation("Парс выполнен успешно");
            
            
            var user = await _repository.GetById(userIdGuid, ct);

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
                var userId = User.FindFirst("userId")?.Value;


                if (userId == null)
                {
                    _logger.LogError("Равен Id null");
                    return BadRequest("Ошибка пользователя. Id равен null");
                }
                _logger.LogInformation("Попытка запарсить id {UserId}", userId);
                
                var userIdGuid =  Guid.Parse(userId);
                
                _logger.LogInformation("Парс выполнен успешно");
                
                _logger.LogInformation("Данные пользователя: {Request}", request);

                await _repository.UpdateInfo(userIdGuid, request.Name!, request.Login!, request.Email!, ct);

                return Ok(userIdGuid);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, "Ошибка обновления данных. Данные {Request}", request);
                throw;
            }
        }
        

        [HttpGet("take-sum")]
        public async Task<ActionResult<decimal>> TakeSum(CancellationToken ct)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (userId == null)
            {
                _logger.LogError("Равен Id null");
                return BadRequest("Ошибка пользователя. Id равен null");
            }
            _logger.LogInformation("Попытка запарсить id {UserId}", userId);
                
            var userIdGuid =  Guid.Parse(userId);
                
            _logger.LogInformation("Парс выполнен успешно");
            
           var result = await  _repository.TakeSum(userIdGuid, ct);

           return Ok(result);
        }

        [HttpGet("take-history")]
        public async Task<IActionResult> TakeHistory(CancellationToken ct)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (userId == null)
            {
                _logger.LogError("Равен Id null");
                return BadRequest("Ошибка пользователя. Id равен null");
            }
            _logger.LogInformation("Попытка запарсить id {UserId}", userId);
                
            var userIdGuid =  Guid.Parse(userId);
                
            _logger.LogInformation("Парс выполнен успешно");
            _logger.LogInformation("{UserId}", userId);
            var result = await _repository.TakeHistory(userIdGuid, ct);
            
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
        public async Task<ActionResult<string>> Login(
            [FromBody] LoginUserRequest request,
            CancellationToken ct)
        {
            var token = await _service.Login(request.Login, request.Password, ct);

            if (token == null)
            {
                _logger.LogError("токен не создался");
                return Unauthorized("Ошибка авторизации. Неверный логин или пароль");
            }
            
            HttpContext.Response.Cookies.Append("tasty-cookies", token, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7)
            });
            
            
            return Ok(token);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Logout()
        {
            HttpContext.Response.Cookies.Delete("tasty-cookies");
            return Ok(true);
        }

    }
}
