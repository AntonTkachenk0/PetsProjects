using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetsProjects.DataBaseAccess.Postgres.Entity;
using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly PetsProjectDbContext _context;

        public PetRepository(PetsProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pet>> Get(CancellationToken ct)
        {
            var petEntity = await _context.Pet
                .AsNoTracking()
                .ToListAsync(ct);


            var results = petEntity
                    .Select(entity => Pet.Create(
                        entity.Id,
                        entity.Name,
                        entity.Breed,
                        entity.Gender,
                        entity.Photo_Id,
                        entity.Id_User
                    ))
                    .ToList();

            return results
                .Where(r => r.IsSuccess)
                .Select(r => r.Value!)
                .OrderByDescending(p => p.Name)
                .ToList();
        }

        public async Task<Guid> Create(Pet pet, CancellationToken ct)
        {
            var petEntity = new PetEntity
            {
                Id = pet.Id,
                Name = pet.Name,
                Gender = pet.Gender,
                Breed = pet.Breed,
                Photo_Id = pet.Photo_Id,
                Id_User = pet.Id_User,
            };

            await _context.Pet.AddAsync(petEntity, ct);
            await _context.SaveChangesAsync(ct);

            return petEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name, string breed, string gender, Guid id_user, CancellationToken ct)
        {

            await _context.Pet
                 .Where(p => p.Id == id)
                 .ExecuteUpdateAsync(setter => setter
                 .SetProperty(p => p.Name, name)
                 .SetProperty(p => p.Breed, breed)
                 .SetProperty(p => p.Gender, gender)
                 .SetProperty(p => p.Id_User, id_user)
                 , ct);

            return id;
        }
        
        public async Task<string> TakePhotoId(Guid user_id, CancellationToken ct)
        {
            var photoId = await _context.Pet
                .Where(u => u.Id == user_id)
                .Select(u => u.Photo_Id)
                .FirstOrDefaultAsync(ct);
            
            return photoId!;
        }    
            
        
        public async Task<Guid> TakeIdUserFromPhotoId(string photo_id, CancellationToken ct)
        {
            var photoId = await _context.Pet
                .Where(u => u.Photo_Id == photo_id)
                .Select(u => u.Id)
                .FirstOrDefaultAsync(ct);
            
            return photoId;
        }
        
        
        public async Task<Guid> UpdatePhoto(Guid id, string photoId, CancellationToken ct)
        {

            await _context.Pet
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(setter => setter
                        .SetProperty(p => p.Photo_Id, photoId)
                    , ct);

            return id;
        }
    }
}
