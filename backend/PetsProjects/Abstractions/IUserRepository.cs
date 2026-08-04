using PetsProjects.Models;

namespace PetsProjects.Abstractions
{
    public interface IUserRepository
    {
        Task<Guid> Create(UserModel user, CancellationToken ct);
        Task<List<UserModel>> Get(CancellationToken ct);
        Task<Guid> Update(Guid id, string name, string login, string email, string passwordHached, CancellationToken ct);
    }
}