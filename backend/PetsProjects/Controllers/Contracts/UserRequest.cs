namespace PetsProjects.Controllers.Contracts
{
    public record UserRequest(
        Guid Id,
        string Name,
        string Login,
        string Email,
        string PasswordHached
        );
}
