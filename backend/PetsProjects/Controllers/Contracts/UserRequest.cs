namespace PetsProjects.Controllers.Contracts
{
    public record UserRequest(
        string Name,
        string Login,
        string Email,
        string PasswordHached
        );
}
