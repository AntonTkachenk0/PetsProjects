using PetsProjects.Models;

namespace PetsProjects.Abstractions
{
    public interface IUserRepository
    {
        Task<Guid> Create(UserModel user);
        Task<List<UserModel>> Get();
        Task<Guid> Update(Guid id, string name, string login, string email, string passwordHached);
    }
}