using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.TrainingSessionModels;

public class TrainingSessionForm
{
 
    public Guid TrainerMemberId { get; set; }

    [Required(ErrorMessage = "Session Name is required")]
    [Display(Name = "Session Name", Prompt = "Enter Session Name")]
    public string SessionName { get; set; } = null!;

    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.DateTime)] // Got help by chatGPT to create a more user-friendly way to add Date and Time to the UI.
    [Display(Name = "Start Date", Prompt = "Enter Start Date")]
    public DateTimeOffset StartDate { get; set; } = new DateTime(2026, 1, 1);

    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.DateTime)]
    [Display(Name = "End Date", Prompt = "Enter End Date")]
    public DateTimeOffset EndDate { get; set; } = new DateTime(2026, 1, 1);

    [Display(Name = "Capacity", Prompt = "Enter Capacity")]
    public int Capacity { get; set; }

    [Required(ErrorMessage = "Location is required")]
    [Display(Name = "Location", Prompt = "Enter Location")]
    public string Location { get; set; } = null!;
}