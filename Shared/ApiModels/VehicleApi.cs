using Shared.Enums;

namespace Shared.ApiModels;

public class VehicleApi
{
    public int Id { get; set; }

    public string LicensePlate { get; set; }

    public int ModelId { get; set; }

    public ModelApi? Model { get; set; } = null;

    public int Year { get; set; }

    public int Kilometers { get; set; }

    public int? NextMaintenanceKilometers { get; set; }

    public Energy Energy { get; set; }

    public IList<MaintenanceApi> Maintenances { get; set; } = new List<MaintenanceApi>();
}