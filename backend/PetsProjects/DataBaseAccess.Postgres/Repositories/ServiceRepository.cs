using Microsoft.EntityFrameworkCore;
using PetsProjects.Abstractions;
using PetsProjects.DataBaseAccess.Postgres.Entity;
using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly PetsProjectDbContext _context;

        public ServiceRepository(PetsProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> Get(CancellationToken ct)
        {
            var serviceEntity = await _context.Service
                .AsNoTracking()
                .ToListAsync(ct);


            var results = serviceEntity
                    .Select(entity => Service.Create(
                        entity.Id,
                        entity.Title,
                        entity.Cost
                    ))
                    .ToList();

            return results
                .Where(r => r.IsSuccess)
                .Select(r => r.Value!)
                .ToList();
        }

        public async Task<Guid> Create(Service service, CancellationToken ct)
        {
            var serviceEntity = new ServiceEntity
            {
                Id = service.Id,
                Title = service.Title,
                Cost = service.Cost
            };

            await _context.Service.AddAsync(serviceEntity, ct);
            await _context.SaveChangesAsync(ct);

            return serviceEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string title, decimal cost, CancellationToken ct)
        {

            await _context.Service
                 .Where(s => s.Id == id)
                 .ExecuteUpdateAsync(setter => setter
                 .SetProperty(s => s.Title, title)
                 .SetProperty(s => s.Cost, cost)
                 , ct);

            return id;
        }
    }
}
