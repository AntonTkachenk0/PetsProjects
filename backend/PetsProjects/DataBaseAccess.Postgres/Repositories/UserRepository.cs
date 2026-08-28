using Microsoft.EntityFrameworkCore;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.DataBaseAccess.Postgres.Entity;
using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PetsProjectDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(PetsProjectDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<UserModel>> Get(CancellationToken ct)
        {
            var userEntity = await _context.User
                .AsNoTracking()
                .ToListAsync(ct);


            var results = userEntity
                    .Select(entity => UserModel.Create(
                        entity.Id,
                        entity.Name,
                        entity.Login,
                        entity.Email,
                        entity.PasswordHached,
                        entity.PhotoS3Key
                    ))
                    .ToList();

            return results
                .Where(r => r.IsSuccess)
                .Select(r => r.Value!)
                .ToList();
        }

        public async Task<UserModel> GetById(Guid id, CancellationToken ct)
        {
            var entity = await _context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);


            var results = UserModel.Create(
                    entity.Id,
                    entity.Name,
                    entity.Login,
                    entity.Email,
                    entity.PasswordHached,
                    entity.PhotoS3Key
                );

            if (results.IsSuccess)
            {
                return results.Value!;
            }

            return null;
        }
        
        public async Task<Guid> Create(UserModel user, CancellationToken ct)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                Name = user.Name,
                Login = user.Login,
                Email = user.Email,
                PasswordHached = user.PasswordHached,
                PhotoS3Key = user.PhotoS3Key,
            };

            await _context.User.AddAsync(userEntity, ct);
            await _context.SaveChangesAsync(ct);

            return userEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name, string login, string email, string passwordHached, CancellationToken ct)
        {

            await _context.User
                 .Where(p => p.Id == id)
                 .ExecuteUpdateAsync(setter => setter
                 .SetProperty(p => p.Name, name)
                 .SetProperty(p => p.Login, login)
                 .SetProperty(p => p.Email, email)
                 .SetProperty(p => p.PasswordHached, passwordHached)
                 , ct);

            return id;
        }

        public async Task<decimal> TakeSum(Guid id_Pet, CancellationToken ct)
        {
            var result = await _context.ServiceJournal
                .Where(u => u.Pets.Id_User == id_Pet)  
                .SumAsync(u => u.Service.Cost, ct);

            return result;
        }

        public async Task<string> TakePhotoId(Guid id_User, CancellationToken ct)
        {
            var photoId = await _context.User
                .Where(u => u.Id == id_User)
                .Select(u => u.PhotoS3Key)
                .FirstOrDefaultAsync(ct);
            
            return photoId;
        }
        
        
        public async Task<Guid> UpdateInfo(Guid id, string name, CancellationToken ct)
        {

            await _context.User
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(setter => setter
                        .SetProperty(p => p.Name, name)
                    , ct);

            return id;
        }
        public async Task<Guid> UpdatePhoto(Guid id, string photoId, CancellationToken ct)
        {

            await _context.User
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(setter => setter
                        .SetProperty(p => p.PhotoS3Key, photoId)
                    , ct);

            return id;
        }

        public async Task<List<ServiceJournalHistoryDto>> TakeHistory(Guid id_User, CancellationToken ct)
        {
            var entities = await _context.ServiceJournal
                .Include(a => a.Pets)
                .Include(a => a.Service)
                .Where(a => a.Pets.Id_User == id_User)
                .AsNoTracking()
                .ToListAsync(ct);
            
            var result =  entities.Select(a => new ServiceJournalHistoryDto 
                (    a.Id_Pet,
                     a.Service.Title,
                    a.VisitDate,
                     a.Pets.Name,
                     a.Service.Cost
                   )
                )
                .ToList();
        

            return result;
        }
       
        
    }
}
