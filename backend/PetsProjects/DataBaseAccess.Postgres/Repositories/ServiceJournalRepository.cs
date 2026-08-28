using Microsoft.EntityFrameworkCore;
using PetsProjects.DataBaseAccess.Postgres.Entity;
using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories;

public class ServiceJournalRepository : IServiceJournalRepository
{
    private readonly PetsProjectDbContext _context;

    public ServiceJournalRepository(PetsProjectDbContext context)
    {
        _context = context;
    }


    public async Task<Guid> Create(ServiceJournal service, CancellationToken ct)
    {
        var journal = new ServiceJournalEntity
        {
            Id = service.Id,
            Id_Pet = service.Id_Pet,
            Id_Service = service.Id_Service,
            VisitDate = service.VisitDate
        };
        
        await _context.ServiceJournal.AddAsync(journal, ct);
        await _context.SaveChangesAsync(ct);
        return journal.Id;
    }

    public async Task<List<ServiceJournal>> GetAll(CancellationToken ct)
    {
        var serviceJournalEntity = await _context.ServiceJournal
            .AsNoTracking()
            .ToListAsync(ct);

        var result = serviceJournalEntity.Select(entity => ServiceJournal.Create(
            entity.Id,
            entity.Id_Pet,
            entity.Id_Service,
            entity.VisitDate))
            .ToList();
        
        return result
            .Where(r => r.IsSuccess)
            .Select(r => r.Value!)
            .ToList();
    }
    
}