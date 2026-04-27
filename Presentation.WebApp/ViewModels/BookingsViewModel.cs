namespace Presentation.WebApp.ViewModels;

public class BookingsViewModel
{
    public Guid Id { get; set; }
    public string? SessionName { get; set; }
    public string? TrainerFirstName { get; set; }
    public string? TrainerLastName { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}
