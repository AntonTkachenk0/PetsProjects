
using PetsProjects.DataBaseAccess.Postgres.Entity;

namespace PetsProjects.Models
{
    
    public class Pet
    {
        public const int MAX_LENGTH_NAME = 120;
        public const int MAX_LENGTH_GENDER = 7;
        public Guid Id { get; }

            public string Name { get;} = string.Empty;

            public string Breed { get;  } = string.Empty;

            public string Gender { get; } = string.Empty;

            public List<ServiceJournal> Services = new();

            public Guid Id_User { get;  }

            public User? User { get; }

        private Pet(Guid id, string name, string breed, string gender, Guid id_User)
        {
            Id = id;
            Name = name;
            Breed = breed;
            Gender = gender;
            Id_User = id_User;
        }
        public (Pet ,string Error) Create(Guid id, string name, string breed, string gender, Guid id_User)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(name))
            {
                error = "Имя не может быть пустым или больше чем 120 символов";
            }
            if (string.IsNullOrEmpty(gender) || gender.Length > MAX_LENGTH_GENDER)
            {
                error = "Имя не может быть пустым или больше чем 120 символов";
            }

            var pet = new Pet(id, name, breed, gender, id_User);


            return (pet, error);
        }
    }
}
