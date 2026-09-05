using PetsProjects.Controllers.Contracts;
using PetsProjects.Models;

namespace PetsProjects.Abstractions
{
    public interface IUserRepository
    {
        Task<Guid> Create(UserModel user, CancellationToken ct);
        Task<List<UserModel>> Get(CancellationToken ct);
        Task<UserModel> GetById(Guid userId, CancellationToken ct);
        Task<UserModel?> GetByLogin(string login, CancellationToken ct);
        Task<Guid> Update(Guid id, string name, string login, string email, string passwordHached, CancellationToken ct);
        Task<decimal> TakeSum(Guid user_Id, CancellationToken ct);
        Task<string> TakePhotoId(Guid id_User, CancellationToken ct);
        Task<Guid> UpdatePhoto(Guid userId, string photoId, CancellationToken ct);
        Task<Guid> UpdateInfo(Guid id, string name, string login, string email, CancellationToken ct);
        Task<List<ServiceJournalHistoryDto>> TakeHistory(Guid id_User, CancellationToken ct);
    }
}