using PetsProjects.Models;

namespace PetsProjects.Abstractions
{
    public interface IServiceRepository
    {
        Task<Guid> Create(Service service);
        Task<List<Service>> Get();
        Task<Guid> Update(Guid id, string title, decimal cost);
    }
}