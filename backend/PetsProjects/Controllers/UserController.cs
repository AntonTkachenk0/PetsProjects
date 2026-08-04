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

        public UserController(IUserRepository repository)
        {
            _repository = repository;   
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> Get(CancellationToken ct)
        {
            var user = await _repository.Get(ct);

            var response = user.Select(p => new UserResponse(p.Id, p.Name, p.Login, p.Email, p.PasswordHached, p.PhotoS3Key));

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
                "PhotoS3Key");

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
            [FromBody] UserRequest request,
            CancellationToken ct)
        {
            await _repository.Update(id, request.Name, request.Login, request.Email, request.PasswordHached,  ct);
            return Ok(id);
        }
    }
}
