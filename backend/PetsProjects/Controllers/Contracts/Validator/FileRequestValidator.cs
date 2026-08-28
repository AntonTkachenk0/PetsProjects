using FluentValidation;

namespace PetsProjects.Controllers.Contracts.Validator;

public class FileRequestValidator 
    : AbstractValidator<FileUploadDto>
{
    private const long MAX_SIZE = 5 * 1024 * 1024;
    
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
    
    private static readonly string[] AllowedContentTypes = { 
        "image/jpeg", 
        "image/png", 
        "image/jpg", 
    };
    
    public FileRequestValidator()
    {
        RuleFor(file => file.File)
            .NotNull()
            .WithMessage("Файл обязателен для загрузки!");
        
        RuleFor(x => x.File)
            .Must(a => a.Length <= MAX_SIZE);

        RuleFor(x => x.File)
            .Must(a => AllowedExtensions.Contains(Path.GetExtension(a.FileName.ToLower())))
            .WithMessage("Недопустимое расширение файла");
        
        RuleFor(x => x.File)
            .Must(a => AllowedContentTypes.Contains(a.ContentType.ToLower()))
            .WithMessage("Недопустимый тип файла");    
        
        RuleFor(x => x.File)
            .Must(a => !Path.GetFileNameWithoutExtension(a.FileName)
                .ContainsAny('/', '\\', ':', '*', '?', '"', '<', '>', '|'))
                .WithMessage("Имя файла содержит недопустимые символы");
        
        RuleFor(x=> x.Role.ToLower())
            .Must(role => role == "user" || role == "pet")
            .WithMessage("Роль должна быть user или pet");
    }

}

public static class StringExtensions
{
    public static bool ContainsAny(this string str, params char[] chars)
    {
        return str.IndexOfAny(chars) != -1;
    }
}