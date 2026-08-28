namespace PetsProjects.Controllers.Contracts;

public class FileUploadDto
{
    public IFormFile? File { get; set; }
    
    public Guid PetOrUserId {get; set;}

    public string Role {get; set;}
}