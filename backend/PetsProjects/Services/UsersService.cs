using PetsProjects.Abstractions;
using PetsProjects.Models;

namespace PetsProjects.Services;

public class UsersService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UsersService(IUserRepository repository,IPasswordHasher passwordHasher, IJwtProvider  jwtProvider )
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    public async Task Register(string name, string login, string email, string password,  CancellationToken ct)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        
        var user = UserModel.Create(
            Guid.NewGuid(),
            name,
            login,
             email,
             hashedPassword,
            "default");
    
        if (!user.IsSuccess)
        {
            Console.WriteLine("Ошибка");
          
        }
        
        await _repository.Create(user.Value!, ct);

    }

    public async Task<string> Login(string email, string password, CancellationToken ct)
    {
        var user = await _repository.GetByEmail(email, ct);

        var result =  _passwordHasher.Verify(password, user.PasswordHached);

        if (!result)
        {
            throw new Exception("Ошибка входа");
        }


        var token = _jwtProvider.GenerateToken(user);
        return token;
    }
}