namespace Shared.ApiModels;

public class MaintenanceApi
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    public int Kilometers { get; set; }
}