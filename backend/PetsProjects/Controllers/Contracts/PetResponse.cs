namespace PetsProjects.Controllers.Contracts
{
    public record PetResponse(
        Guid Id,
        string Name,
        string Breed,
        string Gender,
        string Photo_Id,
        Guid Id_User);
}
