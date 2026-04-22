using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.MembershipPlanModels;

public class MembershipPlanForm
{
    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "Name", Prompt = "Enter Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    [Display(Name = "Description", Prompt = "Enter Description")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "ListItem is required")]
    [Display(Name = "ListItem 1", Prompt = "Enter ListItem 1")]
    public string ListItem1 { get; set; } = null!;

    [Required(ErrorMessage = "ListItem is required")]
    [Display(Name = "ListItem 2", Prompt = "Enter ListItem 2")]
    public string ListItem2 { get; set; } = null!;

    [Required(ErrorMessage = "ListItem is required")]
    [Display(Name = "ListItem 3", Prompt = "Enter ListItem 3")]
    public string ListItem3 { get; set; } = null!;

    [Required(ErrorMessage = "Price is requirerd")]
    [Display(Name = "Price", Prompt = "Enter price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Valid days is required")]
    [Display(Name = "Valid days", Prompt = "Enter valid days")]
    public int ValidDays { get; set; }
}
