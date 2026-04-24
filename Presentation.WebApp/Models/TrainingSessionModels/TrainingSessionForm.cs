using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.TrainingSessionModels;

public class TrainingSessionForm
{
 
    public Guid TrainerMemberId { get; set; }

    [DataType(DataType.DateTime)] // Got help by chatGPT to create a more user-friendly way to add Date and Time to the UI.
    [Display(Name = "Start Date", Prompt = "Enter Start Date")]
    public DateTimeOffset StartDate { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "End Date", Prompt = "Enter End Date")]
    public DateTimeOffset EndDate { get; set; }


    [Display(Name = "Capacity", Prompt = "Enter Capacity")]
    public int Capacity { get; set; }


    [Display(Name = "Location", Prompt = "Enter Location")]
    public string? Location { get; set; }
}