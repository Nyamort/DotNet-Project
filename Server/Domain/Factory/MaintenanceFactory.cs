using Server.Domain.Entity;
using Shared.ApiModels;

namespace Server.Domain.Factory;

public static class MaintenanceFactory
{
    public static MaintenanceApi? ToApiModel(Maintenance? maintenance)
    {
        if (maintenance is null)
        {
            return null;
        }

        return new MaintenanceApi
        {
            Id = maintenance.Id,
            Date = maintenance.Date,
            Kilometers = maintenance.Kilometers,
            Description = maintenance.Description,
            VehicleId = maintenance.VehicleId
        };
    }

    public static IList<MaintenanceApi> ToApiModel(IEnumerable<Maintenance> maintenances)
    {
        return maintenances.Select(maintenance => ToApiModel(maintenance)!).ToList();
    }
}