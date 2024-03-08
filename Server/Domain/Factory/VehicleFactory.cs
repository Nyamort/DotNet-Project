using Server.Domain.Entity;
using Shared.ApiModels;

namespace Server.Domain.Factory;

public static class VehicleFactory
{
    public static VehicleApi? ToApiModel(Vehicle? vehicle)
    {
        if (vehicle is null)
        {
            return null;
        }

        return new VehicleApi
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Model = ModelFactory.ToApiModel(vehicle.Model),
            ModelId = vehicle.ModelId,
            Year = vehicle.Year,
            Kilometers = vehicle.Kilometers,
            Energy = vehicle.Energy,
            NextMaintenanceKilometers = vehicle.Model.MaintenanceFrequency -  vehicle.Kilometers -
                                        (vehicle.Maintenances.Count > 0 ? vehicle.Maintenances.Max(y => y.Kilometers) : 0),
            Maintenances = MaintenanceFactory.ToApiModel(vehicle.Maintenances)
        };
    }

    public static IList<VehicleApi> ToApiModel(IEnumerable<Vehicle> vehicles)
    {
        return vehicles.Select(vehicle => ToApiModel(vehicle)!).ToList();
    }
}