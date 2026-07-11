namespace PetsProjects.Models
{
    public class Service
    {
        public Guid Id { get;  }

        public string Title { get;  } = string.Empty;

        public decimal Cost { get;  } = 0;

        public List<ServiceJournal> JournalService = new();
    }
}
