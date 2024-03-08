using System.ComponentModel.DataAnnotations;

namespace Shared.ApiModels;

public class CreateMaintenanceApi
{
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public int Kilometers { get; set; }
}