namespace PetsProjects.Controllers.Contracts
{
    public record UserResponse(
        Guid id,
        string Name, 
        string Login,
        string Email,
        string PasswordHached, 
        string PhotoS3Key);
}
