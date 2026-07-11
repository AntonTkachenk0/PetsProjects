using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetsProjects.DataBaseAccess.Postgres.Entity;
using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Configuration
{
    public class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Title)
               .HasMaxLength(120)
               .IsRequired();

            builder.Property(s => s.Cost)
               .IsRequired();

            builder.
                HasMany(p => p.JournalService)
                .WithOne(s => s.Service)
                .HasForeignKey(s => s.Id_Service);
        }
    }
}
