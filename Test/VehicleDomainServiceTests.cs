using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Moq;
using Server.Domain.Entity;
using Server.Domain.Service;
using Shared.Enums;

namespace Test;

[TestFixture]
public class VehicleDomainServiceTests
{
    private Mock<DbContext> _mockContext;
    private VehicleDomainService _service;

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<DbContext>();
        _service = new VehicleDomainService(_mockContext.Object);
    }

    [Test]
    public void Validate_ShouldReturnNull_WhenVehicleIsValid()
    {
        _mockContext.Setup(x => x.Set<Model>().Find(1)).Returns(new Model());
        var vehicle = new Vehicle { LicensePlate = "AA-11-11", Kilometers = 100, Year = 2020, ModelId = 1, Energy = Energy.Gasoline };

        var result = _service.Validate(vehicle);

        Assert.IsNull(result);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenLicensePlateIsEmpty()
    {
        var vehicle = new Vehicle { LicensePlate = "", Kilometers = 100, Year = 2020, ModelId = 1, Energy = Energy.Gasoline };

        var result = _service.Validate(vehicle);

        Assert.IsNotNull(result);
        Assert.AreEqual("The license plate is empty", result.Message);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenKilometersIsNegative()
    {
        var vehicle = new Vehicle { LicensePlate = "AA-11-11", Kilometers = -1, Year = 2020, ModelId = 1, Energy = Energy.Gasoline };

        var result = _service.Validate(vehicle);

        Assert.IsNotNull(result);
        Assert.AreEqual("The kilometers is less than 0", result.Message);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenYearIsNegative()
    {
        var vehicle = new Vehicle { LicensePlate = "AA-11-11", Kilometers = 100, Year = -1, ModelId = 1, Energy = Energy.Gasoline };

        var result = _service.Validate(vehicle);

        Assert.IsNotNull(result);
        Assert.AreEqual("The year is less than 0", result.Message);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenLicensePlateHasLessThan7Characters()
    {
        _mockContext.Setup(x => x.Set<Model>().Find(1)).Returns(new Model());
        var vehicle = new Vehicle { LicensePlate = "A-11-1", Kilometers = 100, Year = 2020, ModelId = 1, Energy = Energy.Gasoline };

        var result = _service.Validate(vehicle);

        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Message, Is.EqualTo("The license plate must have 7 to 9 characters"));
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenLicensePlateHasMoreThan9Characters()
    {
        _mockContext.Setup(x => x.Set<Model>().Find(1)).Returns(new Model());
        var vehicle = new Vehicle { LicensePlate = "AAA-11-111", Kilometers = 100, Year = 2020, ModelId = 1, Energy = Energy.Gasoline };

        var result = _service.Validate(vehicle);

        Assert.That(result, Is.Not.Null);
        Assert.AreEqual("The license plate must have 7 to 9 characters", result.Message);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenEnergyIsInvalid()
    {
        var vehicle = new Vehicle { LicensePlate = "AA-11-11", Kilometers = 100, Year = 2020, ModelId = 1, Energy = (Energy) 100 };

        var result = _service.Validate(vehicle);

        Assert.IsNotNull(result);
        Assert.AreEqual("The energy is not valid", result.Message);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenModelIdDoesNotExist()
    {
        _mockContext.Setup(x => x.Set<Model>().Find(1)).Returns((Model) null);
        var vehicle = new Vehicle { LicensePlate = "AA-11-11", Kilometers = 100, Year = 2020, ModelId = 1, Energy = Energy.Gasoline };

        var result = _service.Validate(vehicle);

        Assert.IsNotNull(result);
        Assert.AreEqual("The modelId does not exist in the database", result.Message);
    }
}