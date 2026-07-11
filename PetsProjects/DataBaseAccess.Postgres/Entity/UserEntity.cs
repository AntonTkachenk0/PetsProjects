using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetsProjects.DataBaseAccess.Postgres.Entity
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string PasswordHached { get; set; } = string.Empty;

        public List<PetEntity> Pets = new();
        public string PhotoS3Key { get; set; } = string.Empty;

    }
}
