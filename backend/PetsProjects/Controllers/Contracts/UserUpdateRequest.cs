namespace PetsProjects.Controllers.Contracts;

public record UserUpdateRequest(
    Guid Id,
    string? Name,
    string? Login,
    string? Email
);