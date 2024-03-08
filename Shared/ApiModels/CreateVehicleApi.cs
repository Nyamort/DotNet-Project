using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.ApiModels;

public class CreateVehicleApi
{
    [Required]
    [StringLength(9, MinimumLength = 7)]
    public string LicensePlate { get; set; }

    [Required]
    public int ModelId { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int Kilometers { get; set; }

    [Required]
    public Energy Energy { get; set; }
}