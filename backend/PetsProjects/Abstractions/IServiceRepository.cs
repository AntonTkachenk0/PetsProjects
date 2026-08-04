using PetsProjects.Models;

namespace PetsProjects.Abstractions
{
    public interface IServiceRepository
    {
        Task<Guid> Create(Service service, CancellationToken ct);
        Task<List<Service>> Get(CancellationToken ct);
        Task<Guid> Update(Guid id, string title, decimal cost, CancellationToken ct);
    }
}