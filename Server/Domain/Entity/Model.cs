using Shared.Enums;

namespace Server.Domain.Entity;

public class Model
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Brand Brand { get; set; }

    public IList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public int MaintenanceFrequency { get; set; }
}