namespace PetsProjects.Controllers.Contracts;

public record LoginUserRequest(
    string Login,
    string Email,
    string Password
);