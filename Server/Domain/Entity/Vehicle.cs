using Shared.Enums;

namespace Server.Domain.Entity;

public class Vehicle
{
    public int Id { get; set; }

    public string LicensePlate { get; set; }

    public int ModelId { get; set; }

    public Model Model { get; set; }

    public int Year { get; set; }

    public int Kilometers { get; set; }

    public Energy Energy { get; set; }

    public IList<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
}