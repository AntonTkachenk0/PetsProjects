
using PetsProjects.DataBaseAccess.Postgres.Entity;

namespace PetsProjects.Models
{
    
    public class Pet
    {
        public const int MAX_LENGTH_NAME = 120;
        public const int MAX_LENGTH_GENDER = 7;
        public const int MAX_LENGTH_BREED = 120;
        public Guid Id { get; }

            public string Name { get;} = string.Empty;

            public string Breed { get;  } = string.Empty;

            public string Gender { get; } = string.Empty;
            
            public string? Photo_Id { get; set; } = string.Empty;

            public List<ServiceJournal> Services = new();

            public Guid Id_User { get;  }

            public UserModel? User { get; }

        private Pet(Guid id, string name, string breed, string gender,  string photo_Id, Guid id_User)
        {
            Id = id;
            Name = name;
            Breed = breed;
            Gender = gender;
            Photo_Id = photo_Id;
            Id_User = id_User;
        }
        public static Result<Pet> Create(Guid id, string name, string breed, string gender, string? photo_Id , Guid id_User)
        {

            if (string.IsNullOrEmpty(name) || name.Length > MAX_LENGTH_NAME)
            {
               return Result<Pet>.Failure("Имя не может быть пустым или больше чем 120 символов");
            }
            if (string.IsNullOrEmpty(gender) || gender.Length > MAX_LENGTH_GENDER)
            {
                return Result<Pet>.Failure("Пол не может быть пустым или больше чем 7 символов");
            }
            if (string.IsNullOrEmpty(breed) || breed.Length > MAX_LENGTH_BREED)
            {
                return Result<Pet>.Failure("Кличка не может быть пустой");
            }
            if(id_User == Guid.Empty)
            {
                return Result<Pet>.Failure("Нет хозяина");
            }
            if (string.IsNullOrEmpty(photo_Id))
            {
                photo_Id = "defaultPet";
            }


            var pet = new Pet(id, name, breed, gender, photo_Id, id_User);


            return Result<Pet>.Success(pet);
        }
    }
}
