using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Entity;
using Shared.Enums;

namespace Server.Domain.Service;

public class VehicleDomainService(DbContext context)
{
    private DbSet<Model> ModelRepository => context.Set<Model>();

    public ValidationException? Validate(Vehicle vehicle)
    {
        if (string.IsNullOrWhiteSpace(vehicle.LicensePlate))
            return new ValidationException("The license plate is empty");

        if (vehicle.Kilometers < 0)
            return new ValidationException("The kilometers is less than 0");

        if (vehicle.Year < 0)
            return new ValidationException("The year is less than 0");

        if (vehicle.LicensePlate.Length is < 7 or > 9)
            return new ValidationException("The license plate must have 7 to 9 characters");

        if(Enum.IsDefined(typeof(Energy), vehicle.Energy) == false)
            return new ValidationException("The energy is not valid");

        if (ModelRepository.Find(vehicle.ModelId) is null)
            return new ValidationException("The modelId does not exist in the database");

        return null;
    }
}