using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetsProjects.DataBaseAccess.Postgres.Entity
{
    public class ServiceJournalEntity
    {
        public Guid Id { get; set; }

        public DateTime VisitDate { get; set; } = DateTime.UtcNow;

        public Guid Id_Pet { get; set; }

        public PetEntity? Pets { get; set; }

        public Guid Id_Service { get; set; }

        public ServiceEntity? Service { get; set; }

    }

}
