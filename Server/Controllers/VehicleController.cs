using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Entity;
using Server.Domain.Factory;
using Server.Domain.Service;
using Shared.ApiModels;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController(
    DbContext context,
    ILogger<VehicleController> logger,
    VehicleDomainService vehicleDomainService,
    MaintenanceDomainService maintenanceDomainService
    )
    : ControllerBase
{
    private DbSet<Vehicle> VehicleRepository => context.Set<Vehicle>();

    [HttpGet]
    [ProducesResponseType(typeof(List<ModelApi>),200)]
    [ProducesResponseType(204)]
    public IActionResult GetVehicle()
    {
        var models = VehicleRepository.Include(x => x.Model)
            .OrderBy(x => x.LicensePlate)
            .ToList();

        if (models.Count != 0) return Ok(VehicleFactory.ToApiModel(models));
        logger.LogWarning("No models found");
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpGet("Late")]
    [ProducesResponseType(typeof(List<ModelApi>),200)]
    [ProducesResponseType(204)]
    public IActionResult GetLateForMaintenance()
    {

        var models = VehicleRepository.Include(x => x.Model)
            .Include(x => x.Maintenances)
            .Where(x => x.Model.MaintenanceFrequency < x.Kilometers -
                (x.Maintenances.Any() ? x.Maintenances.Max(y => y.Kilometers) : 0))
            .OrderBy(x => x.LicensePlate)
            .ToList();
        if (models.Count != 0) return Ok(VehicleFactory.ToApiModel(models));
        logger.LogWarning("No models found");
        return StatusCode(StatusCodes.Status204NoContent);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ModelApi),200)]
    [ProducesResponseType(404)]
    public IActionResult GetVehicleById(int id)
    {
        var vehicle = VehicleRepository.Include(x => x.Model)
            .Include(x => x.Maintenances)
            .FirstOrDefault(x => x.Id == id);
        if (vehicle is null)
        {
            logger.LogWarning($"No vehicle found with Id: {id}");
            return StatusCode(404, new { message = "Vehicle not found" });
        }
        return Ok(VehicleFactory.ToApiModel(vehicle));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ModelApi),200)]
    [ProducesResponseType(400)]
    public IActionResult CreateVehicle([FromBody] CreateVehicleApi vehicle)
    {
        var newVehicle = new Vehicle
        {
            LicensePlate = vehicle.LicensePlate,
            ModelId = vehicle.ModelId,
            Year = vehicle.Year,
            Kilometers = vehicle.Kilometers,
            Energy = vehicle.Energy
        };

        var error = vehicleDomainService.Validate(newVehicle);
        if (error is not null)
        {
            logger.LogWarning(error.Message);
            return StatusCode(400, new { message = error.Message });

        }

        VehicleRepository.Add(newVehicle);
        context.SaveChanges();
        return Ok(VehicleFactory.ToApiModel(newVehicle));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ModelApi),200)]
    [ProducesResponseType(400)]
    public IActionResult EditVehicle([FromBody] EditVehicleApi vehicleToEdit, int id)
    {

        var dbVehicle = VehicleRepository.FirstOrDefault(x => x.Id == id);

        if (dbVehicle == null)
        {
            logger.LogWarning($"No vehicle found with Id: {id}");
            return StatusCode(404, new { message = "Vehicle not found" });
        }

        dbVehicle.LicensePlate = vehicleToEdit.LicensePlate;
        dbVehicle.ModelId = vehicleToEdit.ModelId;
        dbVehicle.Year = vehicleToEdit.Year;
        dbVehicle.Kilometers = vehicleToEdit.Kilometers;
        dbVehicle.Energy = vehicleToEdit.Energy;

        var error = vehicleDomainService.Validate(dbVehicle);
        if (error is not null)
        {
            logger.LogWarning(error.Message);
            return StatusCode(400, new { message = error.Message });
        }

        VehicleRepository.Update(dbVehicle);

        context.SaveChanges();
        logger.LogInformation($"The vehicle with Id: {dbVehicle.Id} and license plate: {dbVehicle.LicensePlate} has been edited");
        return Ok(VehicleFactory.ToApiModel(dbVehicle));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteVehicle(int id)
    {
        var vehicle = VehicleRepository.FirstOrDefault(x => x.Id == id);
        if (vehicle is null)
        {
            logger.LogWarning($"No vehicle found with Id: {id}");
            return StatusCode(404, new { message = "Vehicle not found" });
        }
        VehicleRepository.Remove(vehicle);
        context.SaveChanges();
        logger.LogInformation($"The vehicle with Id: {vehicle.Id} and license plate: {vehicle.LicensePlate} has been deleted");
        return StatusCode(204);
    }

    [HttpPost("{id}/Maintenance")]
    [ProducesResponseType(typeof(MaintenanceApi),200)]
    [ProducesResponseType(400)]
    public IActionResult CreateMaintenance(int id, [FromBody] CreateMaintenanceApi maintenance)
    {
        var vehicle = VehicleRepository.Include(x => x.Model)
            .Include(x => x.Maintenances)
            .FirstOrDefault(x => x.Id == id);
        if (vehicle is null)
        {
            logger.LogWarning($"No vehicle found with Id: {id}");
            return StatusCode(404, new { message = "Vehicle not found" });
        }

        var newMaintenance = new Maintenance
        {
            Description = maintenance.Description,
            Date = maintenance.Date,
            Kilometers = maintenance.Kilometers
        };
        vehicle.Maintenances.Add(newMaintenance);

        var error = maintenanceDomainService.Validate(newMaintenance);
        if (error is not null)
        {
            logger.LogWarning(error.Message);
            return StatusCode(400, new { message = error.Message });
        }

        context.SaveChanges();
        return Ok(MaintenanceFactory.ToApiModel(newMaintenance));
    }
}