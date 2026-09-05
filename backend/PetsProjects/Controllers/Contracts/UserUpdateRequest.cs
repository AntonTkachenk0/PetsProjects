namespace PetsProjects.Controllers.Contracts;

public record UserUpdateRequest(
    string? Name,
    string? Login,
    string? Email
);