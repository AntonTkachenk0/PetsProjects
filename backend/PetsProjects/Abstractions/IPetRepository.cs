using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories
{
    public interface IPetRepository
    {
        Task<Guid> Create(Pet pet, CancellationToken ct);
        Task<List<Pet>> GetForCurrentUser(Guid id, CancellationToken ct);
        Task<Guid> Update(Guid id, string name, string breed, string gender, Guid id_user, CancellationToken ct);

        Task<string> TakePhotoId(Guid? user_id, CancellationToken ct);
        Task<Guid> TakeIdUserFromPhotoId(string photo_id, CancellationToken ct);

        Task<Guid?> UpdatePhoto(Guid? id, string photoId, CancellationToken ct);
    }
}