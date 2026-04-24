using Presentation.WebApp.Models.TrainingSessionModels;

namespace Presentation.WebApp.ViewModels;

public class TrainingSessionViewModel
{
    public Guid Id { get; set; }
    public List<MemberListViewModel> Members { get; set; } = new();
    public PromoteToTrainerForm PromoteForm { get; set; } = new();
    public TrainingSessionForm Form { get; set; } = new();
    public List<TrainingSessionListViewModel> Sessions { get; set; } = new();
}