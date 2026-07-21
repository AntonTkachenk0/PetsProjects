using System.Xml.Linq;

namespace PetsProjects.Models
{
    public class Service
    {
        public const int MAX_LENGTH_TITLE = 120;
        public Guid Id { get;  }

        public string Title { get;  } = string.Empty;

        public decimal Cost { get;  } = 0;

        public List<ServiceJournal> JournalService = new();

        private Service(Guid id, string title, decimal cost)
        {
            Id = id;
            Title = title;
            Cost = cost;
        }

        public static Result<Service> Create(Guid id, string title, decimal cost)
        {
            if(id == Guid.Empty)
            {
                return Result<Service>.Failure("Id услуги не может быть пустым");
            }

            if (string.IsNullOrEmpty(title) || title.Length > MAX_LENGTH_TITLE)
            {
                return Result<Service>.Failure($"Названиие не может быть пустым или больше чем {MAX_LENGTH_TITLE} символов");
            }

            if(cost < 0)
            {
                return Result<Service>.Failure("Стоимость не может быть меньше нуля");
            }

           
            var service = new Service(id, title, cost);

            return Result<Service>.Success(service);

        }
    }
}
