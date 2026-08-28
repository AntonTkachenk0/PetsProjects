namespace PetsProjects.Models
{
    public class ServiceJournal
    {
        public Guid Id { get;}

        public DateTime VisitDate { get; }

        public Guid Id_Pet { get;  }

        public Pet? Pets { get; }

        public Guid Id_Service { get; }

        public Service? Service { get; }

        private ServiceJournal(Guid id, Guid id_Pet, Guid id_Service, DateTime visitDate)
        {
            Id = id;
            Id_Pet = id_Pet;
            Id_Service = id_Service;
            VisitDate = visitDate;
        }

        public static Result<ServiceJournal> Create(Guid id, Guid id_Pet, Guid id_Service, DateTime visitDate)
        {
            Console.WriteLine(id_Pet);
            Console.WriteLine(id);
            Console.WriteLine(id_Service);
            Console.WriteLine(visitDate);
            
            if (Guid.Empty == id_Pet || Guid.Empty == id_Service)
            {
                Console.WriteLine("ошибка1");
                return Result<ServiceJournal>.Failure($"Id не может быть пустым pet - {id_Pet}, service -{id_Service}");
            }

            if (visitDate.Date < DateTime.Now.Date)
            {
                Console.WriteLine("ошибка2");
                return Result<ServiceJournal>.Failure($"Дата не может быть прошлой");
            }
            if (visitDate.Hour < 9 || visitDate.Hour > 20)
            {
                Console.WriteLine("ошибка3");
                return Result<ServiceJournal>.Failure("Недопустимое время");
            }
            
            Console.WriteLine("нет");
            var journal = new ServiceJournal(id,  id_Pet, id_Service, visitDate);

            return Result<ServiceJournal>.Success(journal);
        }
        
    }
}
