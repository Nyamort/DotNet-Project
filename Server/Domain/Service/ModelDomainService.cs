using System.ComponentModel.DataAnnotations;
using Server.Domain.Entity;
using Shared.Enums;

namespace Server.Domain.Service;

public class ModelDomainService
{


    public ValidationException? Validate(Model model)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
            return new ValidationException("The name is empty");

        if (model.MaintenanceFrequency < 0)
            return new ValidationException("The maintenance frequency is less than 0");

        if(Enum.IsDefined(typeof(Brand), model.Brand) == false)
            return new ValidationException("The brand is not valid");
        return null;
    }
}