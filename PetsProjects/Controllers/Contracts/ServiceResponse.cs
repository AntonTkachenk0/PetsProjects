namespace PetsProjects.Controllers.Contracts
{
    public record ServiceResponse(
        Guid Id,
        string Title,
        decimal Cost);
}
