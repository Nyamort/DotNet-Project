using Server.Domain.Entity;
using Server.Domain.Service;
using Shared.Enums;

namespace Test;

[TestFixture]
public class ModelDomainServiceTests
{
    private ModelDomainService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new ModelDomainService();
    }

    [Test]
    public void Validate_ReturnsNull_WhenModelIsValid()
    {
        var model = new Model
        {
            Name = "Model1",
            MaintenanceFrequency = 5000,
            Brand = Brand.Audi
        };

        var result = _service.Validate(model);

        Assert.IsNull(result);
    }

    [Test]
    public void Validate_ReturnsException_WhenNameIsEmpty()
    {
        var model = new Model
        {
            Name = "",
            MaintenanceFrequency = 5000,
            Brand = Brand.Audi
        };

        var result = _service.Validate(model);

        Assert.IsNotNull(result);
        Assert.AreEqual("The name is empty", result.Message);
    }

    [Test]
    public void Validate_ReturnsException_WhenMaintenanceFrequencyIsLessThanZero()
    {
        var model = new Model
        {
            Name = "Model1",
            MaintenanceFrequency = -1,
            Brand = Brand.Audi
        };

        var result = _service.Validate(model);

        Assert.IsNotNull(result);
        Assert.AreEqual("The maintenance frequency is less than 0", result.Message);
    }

    [Test]
    public void Validate_ReturnsException_WhenBrandIsNotValid()
    {
        var model = new Model
        {
            Name = "Model1",
            MaintenanceFrequency = 5000,
            Brand = (Brand)100
        };

        var result = _service.Validate(model);

        Assert.IsNotNull(result);
        Assert.AreEqual("The brand is not valid", result.Message);
    }
}