using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
using Server.Domain.Entity;
using Server.Domain.Factory;
using Server.Domain.Service;
using Shared.ApiModels;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModelController : ControllerBase
{

    private readonly DbContext _dataContext;
    private readonly ILogger<ModelController> _logger;

    private DbSet<Model> ModelRepository => _dataContext.Set<Model>();
    private ModelDomainService ModelDomainService;

    public ModelController(
        DbContext context,
        ILogger<ModelController> logger,
        ModelDomainService modelDomainService
        )
    {
        _dataContext = context;
        _logger = logger;
        this.ModelDomainService = modelDomainService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ModelApi>),200)]
    [ProducesResponseType(204)]
    public IActionResult GetModels()
    {
        var models = ModelRepository.ToList();
        if (models.Count != 0) return Ok(ModelFactory.ToApiModel(models));
        _logger.LogWarning("No models found");
        return StatusCode(204);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ModelApi),200)]
    [ProducesResponseType(400)]
    public IActionResult CreateModel([FromBody] CreateModelApi model)
    {
        var newModel = new Model
        {
            Name = model.Name,
            Brand = model.Brand,
            MaintenanceFrequency = model.MaintenanceFrequency
        };

        var error = ModelDomainService.Validate(newModel);
        if (error is not null)
        {
            _logger.LogWarning("Failed to create model");
            return StatusCode(400, new { message = error.Message });
        }

        ModelRepository.Add(newModel);
        _dataContext.SaveChanges();
        return Ok(ModelFactory.ToApiModel(newModel));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteModel(int id)
    {
        var model = ModelRepository.FirstOrDefault(x => x.Id == id);
        if (model is null)
        {
            return StatusCode(404, new { message = "Model not found" });
        }

        ModelRepository.Remove(model);
        _dataContext.SaveChanges();
        return StatusCode(204);
    }
}