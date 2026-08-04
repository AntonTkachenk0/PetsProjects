namespace PetsProjects.Controllers.Contracts
{
    public record PetResponse(
        Guid Id,
        string Name,
        string Breed,
        string Gender,
        Guid Id_User);
}
