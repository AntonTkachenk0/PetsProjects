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
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null!;
            }
        }

        public async Task<UserModel> GetByEmail(string email, CancellationToken ct)
        {
            try
            {
                var entity = await _context.User
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == email, ct);
                
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
                    _logger.LogInformation($"Успешное получения пользователя по id с {email}");
                    return results.Value!;
                }
                _logger.LogError(results.Error);
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<UserModel> GetById(Guid userId, CancellationToken ct)
        {
            try
            {
                var entity = await _context.User
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId, ct);
            

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
                    _logger.LogInformation($"Успешное получения пользователя по id с {userId}");
                    return results.Value!;
                }
                
                _logger.LogError(results.Error);
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ошибка. Id {Id}", userId);
                throw;
            }
        }
        
        public async Task<Guid> Create(UserModel user, CancellationToken ct)
        {
            try
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
                
                _logger.LogInformation("Пользователь создан с id: {UserId}", userEntity.Id);
                return userEntity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ошибка. Id {UserId}", user.Id);
                throw;
            }
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

        public async Task<decimal> TakeSum(Guid userId, CancellationToken ct)
        {
            try
            {

                var result = await _context.ServiceJournal
                    .Where(u => u.Pets != null && u.Pets.Id_User == userId)
                    .SumAsync(u => u.Service!.Cost, ct);

                _logger.LogInformation("Сумма получена для пользователя с id {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ошибка.  Id пользователя {UserId}", userId);
                throw;
            }
        }

        public async Task<string> TakePhotoId(Guid idUser, CancellationToken ct)
        {
            try
            {
                var photoId = await _context.User
                    .Where(u => u.Id == idUser)
                    .Select(u => u.PhotoS3Key)
                    .FirstOrDefaultAsync(ct);
                
                _logger.LogInformation("Фото получено для пользователя с id  {UserId}. Id photo {PhotoId}", idUser, photoId);
                return photoId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ошибка. Id пользователя {Id}", idUser);
                throw;
            }
        }
        
        
        public async Task<Guid> UpdateInfo(Guid id, string? name, string? login, string? email, CancellationToken ct)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id, ct);
        
                if (name != null) user.Name = name;
                if (login != null) user.Login = login;
                if (email != null) user.Email = email;

                await _context.SaveChangesAsync(ct);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ошибка. Id {Id} {Name} {Login} {Email}", id,name, login, email);
                throw;
            }
        }
        public async Task<Guid> UpdatePhoto(Guid userId, string photoId, CancellationToken ct)
        {

            try
            {
                await _context.User
                    .Where(p => p.Id == userId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.PhotoS3Key, photoId), ct);
                    
                _logger.LogInformation("Фото обновлено. Id фото {PhotoId}, Id пользователя {UserId} ", photoId, userId);
                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ошибка. Id {UserId}", userId);
                throw;
            }
        }
        
        public async Task<List<ServiceJournalHistoryDto>> TakeHistory(Guid idUser, CancellationToken ct)
        {
            try
            {


                var entities = await _context.ServiceJournal
                    .Include(a => a.Pets)
                    .Include(a => a.Service)
                    .Where(a => a.Pets != null && a.Pets.Id_User == idUser)
                    .AsNoTracking()
                    .ToListAsync(ct);

                var result = entities.Select(a => new ServiceJournalHistoryDto
                        (a.Id_Pet,
                            a.Service.Title,
                            a.VisitDate,
                            a.Pets.Name,
                            a.Service.Cost
                        )
                    )
                    .ToList();

                _logger.LogInformation("История успешно получена. Id {IdUser}", idUser);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка. Id пользователя {IdUser}", idUser);
                throw;
            }
        }
       
        
    }
}
