namespace PetsProjects.Controllers.Contracts;

public record RegisterUserRequest(
    string Name,
    string Login,
    string Email,
    string Password
);