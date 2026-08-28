namespace PetsProjects.Controllers.Contracts;

public record ServiceJournalRequest(
    Guid Id_Pet,
    Guid Id_Service,
    DateTime VisitDate);