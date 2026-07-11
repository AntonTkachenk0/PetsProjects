using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetsProjects.DataBaseAccess.Postgres.Configuration;
using PetsProjects.DataBaseAccess.Postgres.Entity;

namespace PetsProjects.DataBaseAccess.Postgres
{
    public class PetsProjectDbContext(DbContextOptions<PetsProjectDbContext> options) : DbContext(options)
    {
        DbSet<PetEntity> Pet {  get; set; }
        DbSet<ServiceJournalEntity> ServiceJournal { get; set; }
        
        DbSet<UserEntity> User { get; set; }

        DbSet<ServiceEntity> Service { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PetConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceJournalConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
