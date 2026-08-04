using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories
{
    public interface IPetRepository
    {
        Task<Guid> Create(Pet pet, CancellationToken ct);
        Task<List<Pet>> Get(CancellationToken ct);
        Task<Guid> Update(Guid id, string name, string breed, string gender, Guid id_user, CancellationToken ct);
    }
}