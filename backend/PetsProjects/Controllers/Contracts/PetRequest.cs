namespace PetsProjects.Controllers.Contracts
{
    public record PetRequest(
        string Name,
        string Breed,
        string Gender);
}
