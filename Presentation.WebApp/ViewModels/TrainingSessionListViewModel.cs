namespace Presentation.WebApp.ViewModels;

public class TrainingSessionListViewModel
{
    public Guid Id { get; set; }
    public Guid TrainerMemberId { get; set; }
    public string? TrainerFirstName { get; set; }
    public string? TrainerLastName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; } = null!;
}