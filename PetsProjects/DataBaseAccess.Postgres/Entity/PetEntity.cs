using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace PetsProjects.DataBaseAccess.Postgres.Entity
{
    public class PetEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Breed { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public List<ServiceJournalEntity> Services = new();

        public Guid Id_User { get; set; }
        
        public UserEntity? User { get; set; }

    }
}

