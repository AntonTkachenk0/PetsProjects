using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetsProjects.DataBaseAccess.Postgres.Entity;

namespace PetsProjects.DataBaseAccess.Postgres.Configuration
{
    public class ServiceJournalConfiguration : IEntityTypeConfiguration<ServiceJournalEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceJournalEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.VisitDate)
                .IsRequired();

            builder.Property(p => p.Id_Pet)
                .IsRequired();

            builder.HasOne(s => s.Pets)
                .WithMany(p => p.Services)
                .HasForeignKey(s => s.Id_Pet);

            builder.HasOne(s => s.Service)
                .WithMany(p => p.JournalService)
                .HasForeignKey(s => s.Id_Service);
        }
    }
}
