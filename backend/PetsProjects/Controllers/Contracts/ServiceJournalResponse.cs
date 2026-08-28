namespace PetsProjects.Controllers.Contracts;

public record ServiceJournalResponse(
    Guid Id,
    Guid Id_Pet,
    Guid Id_Service,
    DateTime VisitDate);