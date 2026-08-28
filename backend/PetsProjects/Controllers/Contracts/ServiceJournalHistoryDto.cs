namespace PetsProjects.Controllers.Contracts;

public record  ServiceJournalHistoryDto(
    Guid PetId,
    string ServiceTitle,
    DateTime VisitDate,
    string PetName,
    decimal ServiceCost);

