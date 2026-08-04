using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetsProjects.DataBaseAccess.Postgres.Configuration;
using PetsProjects.DataBaseAccess.Postgres.Entity;

namespace PetsProjects.DataBaseAccess.Postgres
{
    public class PetsProjectDbContext(DbContextOptions<PetsProjectDbContext> options) : DbContext(options)
    {
        public DbSet<PetEntity> Pet {  get; set; }
        public DbSet<ServiceJournalEntity> ServiceJournal { get; set; }

        public DbSet<UserEntity> User { get; set; }

        public DbSet<ServiceEntity> Service { get; set; }

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
