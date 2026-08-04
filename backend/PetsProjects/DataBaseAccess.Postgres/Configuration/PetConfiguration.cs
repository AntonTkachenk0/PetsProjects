using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetsProjects.DataBaseAccess.Postgres.Entity;
using PetsProjects.Models;

namespace PetsProjects.DataBaseAccess.Postgres.Configuration
{
    public class PetConfiguration : IEntityTypeConfiguration<PetEntity>
    {
        public void Configure(EntityTypeBuilder<PetEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(Pet.MAX_LENGTH_NAME)
                .IsRequired();

            builder.Property(p => p.Breed)
               .HasMaxLength(120)
               .IsRequired();

            builder.Property(p => p.Gender)
               .HasMaxLength(Pet.MAX_LENGTH_GENDER)
               .IsRequired();

            builder.Property(p => p.Id_User)
               .IsRequired();

            builder.
                HasMany(p => p.Services)
                .WithOne(s => s.Pets)
                .HasForeignKey(s => s.Id_Pet);

            builder.
                HasOne(p => p.User)
                .WithMany(u => u.Pets);
        }
    }
}
