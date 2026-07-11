namespace PetsProjects.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string PasswordHached { get; set; } = string.Empty;

        public List<Pet> Pets = new();
        public string PhotoS3Key { get; set; } = string.Empty;

    }
}
