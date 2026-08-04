using Microsoft.EntityFrameworkCore;
using PetsProjects.Abstractions;
using PetsProjects.DataBaseAccess.Postgres.Entity;
using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PetsProjectDbContext _context;

        public UserRepository(PetsProjectDbContext context)
        {
            _context = context;
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
    }
}
