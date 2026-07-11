namespace PetsProjects.Models
{
    public class ServiceJournal
    {
        public Guid Id { get;}

        public DateTime VisitDate { get; } = DateTime.UtcNow;

        public Guid Id_Pet { get;  }

        public Pet? Pets { get; }

        public Guid Id_Service { get; }

        public Service? Service { get; }

    }
}
