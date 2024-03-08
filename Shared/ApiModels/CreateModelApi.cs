using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.ApiModels;

public class CreateModelApi
{
    [Required]
    public string Name { get; set; }
    [Required]
    public Brand Brand { get; set; }
    [Required]
    public int MaintenanceFrequency { get; set; }
}