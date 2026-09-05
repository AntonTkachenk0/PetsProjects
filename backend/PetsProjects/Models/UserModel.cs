using System.Reflection;
using System.Xml.Linq;

namespace PetsProjects.Models
{
    public class UserModel
    {
        public const int MAX_LENGTH_NAME = 120;
        
        public const int MAX_LENGTH_CONST= 256;
        public Guid Id { get;  }

        public string Name { get; } = string.Empty;

        public string Login { get;  } = string.Empty;

        public string Email { get; } = string.Empty;
        public string PasswordHached { get; } = string.Empty;

        public List<Pet> Pets = new();
        public string PhotoS3Key { get; } = string.Empty;

      
        private UserModel(Guid id, string name, string login, string email, string passwordHached, string photoS3Key)
        {
            Id = id; 
            Name = name; 
            Login = login; 
            Email = email;
            PasswordHached = passwordHached;
            PhotoS3Key = photoS3Key;
        }

        public static Result<UserModel> Create(Guid id, string name, string login, string email, string passwordHached, string photoS3Key)
        {
            if (string.IsNullOrEmpty(name) || name.Length > MAX_LENGTH_NAME)
            {
                return Result<UserModel>.Failure("Имя не может быть пустым или больше чем 120 символов");
            }
            if (string.IsNullOrEmpty(login) || login.Length > MAX_LENGTH_CONST)
            {
                return Result<UserModel>.Failure($"Логин не может быть пустым или больше чем {MAX_LENGTH_CONST}");
            }
            if (string.IsNullOrEmpty(email) || email.Length > MAX_LENGTH_CONST)
            {
                return Result<UserModel>.Failure("Почта не может быть пустой");
            }
            if (string.IsNullOrEmpty(passwordHached) || passwordHached.Length > MAX_LENGTH_CONST)
            {
                return Result<UserModel>.Failure("Пароль не может быть пустой ");
            }
            if (string.IsNullOrEmpty(photoS3Key))
            {
                photoS3Key = "default";
            }

            var user = new UserModel(id, name, login, email, passwordHached, photoS3Key);

            return Result<UserModel>.Success(user);
        }
    }
}
