using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories;

public interface IServiceJournalRepository
{
    Task<Guid> Create(ServiceJournal service, CancellationToken ct);
    Task<List<ServiceJournal>> GetAll(CancellationToken ct);
}