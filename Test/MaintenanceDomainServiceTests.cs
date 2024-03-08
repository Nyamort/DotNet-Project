using Server.Domain.Entity;
using Server.Domain.Service;

namespace Test;

[TestFixture]
public class MaintenanceDomainServiceTests
{
    private MaintenanceDomainService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new MaintenanceDomainService();
    }

    [Test]
    public void Validate_ShouldReturnNull_WhenMaintenanceIsValid()
    {
        var maintenance = new Maintenance { Kilometers = 100, Description = "Test Description" };

        var result = _service.Validate(maintenance);

        Assert.IsNull(result);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenKilometersIsNegative()
    {
        var maintenance = new Maintenance { Kilometers = -1, Description = "Test Description" };

        var result = _service.Validate(maintenance);

        Assert.IsNotNull(result);
        Assert.AreEqual("Kilometers cannot be negative", result.Message);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenDescriptionIsEmpty()
    {
        var maintenance = new Maintenance { Kilometers = 100, Description = "" };

        var result = _service.Validate(maintenance);

        Assert.IsNotNull(result);
        Assert.AreEqual("Description cannot be empty", result.Message);
    }

    [Test]
    public void Validate_ShouldReturnValidationException_WhenDescriptionIsWhitespace()
    {
        var maintenance = new Maintenance { Kilometers = 100, Description = "   " };

        var result = _service.Validate(maintenance);

        Assert.IsNotNull(result);
        Assert.AreEqual("Description cannot be empty", result.Message);
    }
}