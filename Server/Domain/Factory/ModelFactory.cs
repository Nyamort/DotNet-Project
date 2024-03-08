using Server.Domain.Entity;
using Shared.ApiModels;

namespace Server.Domain.Factory;

public class ModelFactory
{
    public static ModelApi? ToApiModel(Model? model)
    {
        if (model is null)
        {
            return null;
        }

        return new ModelApi
        {
            Id = model.Id,
            Name = model.Name,
            Brand = model.Brand,
            MaintenanceFrequency = model.MaintenanceFrequency
        };
    }

    public static IList<ModelApi> ToApiModel(IEnumerable<Model> models)
    {
        return models.Select(model => ToApiModel(model)!).ToList();
    }
}