using PetsProjects.Models;

namespace PetsProjects.Abstractions
{
    public interface IPetRepository
    {
        Task<Guid> Create(Pet pet);
        Task<List<Pet>> Get();
    }
}