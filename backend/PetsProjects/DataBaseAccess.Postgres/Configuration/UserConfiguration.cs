using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetsProjects.DataBaseAccess.Postgres.Entity;

namespace PetsProjects.DataBaseAccess.Postgres.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(u => u.Login)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(u => u.Email)
               .HasMaxLength(256)
               .IsRequired();

            builder.Property(u => u.PasswordHached)
             .HasMaxLength(256)
             .IsRequired();

            builder.Property(u => u.PhotoS3Key)
             .HasMaxLength(256)
             .IsRequired();

            builder.
                HasMany(u => u.Pets)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.Id_User);
        }
    }
}
