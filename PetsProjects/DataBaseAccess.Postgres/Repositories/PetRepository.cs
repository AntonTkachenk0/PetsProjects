using Microsoft.EntityFrameworkCore;
using PetsProjects.DataBaseAccess.Postgres.Entity;
using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories
{
    public class PetRepository
    {
        private readonly PetsProjectDbContext _context;

        public PetRepository(PetsProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pet>> Get()
        {
            var petEntity = await _context.Pet
                .AsNoTracking()
                .ToListAsync();


            var results = petEntity
                    .Select(entity => Pet.Create(
                        entity.Id,
                        entity.Name,
                        entity.Breed,
                        entity.Gender,
                        entity.Id_User
                    ))
                    .ToList();

            return results
                .Where(r => r.IsSuccess)
                .Select(r => r.Value!)
                .ToList();
        }

        public async Task<Guid> Create(Pet pet)
        {
            var petEntity = new PetEntity
            {
                Id = pet.Id,
                Name = pet.Name,
                Gender = pet.Gender,
                Breed = pet.Breed,
                Id_User = pet.Id_User,
            };

            await _context.Pet.AddAsync(petEntity);
            await _context.SaveChangesAsync();
             
            return petEntity.Id;
        }
    }
}
