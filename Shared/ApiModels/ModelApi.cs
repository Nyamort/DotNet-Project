using Shared.Enums;

namespace Shared.ApiModels;

public class ModelApi
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Brand Brand { get; set; }

    public int MaintenanceFrequency { get; set; }
}