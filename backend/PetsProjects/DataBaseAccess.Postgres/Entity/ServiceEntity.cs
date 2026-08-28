using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetsProjects.DataBaseAccess.Postgres.Entity
{
    public class ServiceEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Cost { get; set; } = 0;

        public List<ServiceJournalEntity> JournalService  { get; set; } = new();
    }

}
