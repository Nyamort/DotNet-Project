using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Entity;
using Shared.Enums;

namespace Server.Domain.Service;

public class MaintenanceDomainService
{


    public ValidationException? Validate(Maintenance maintenance)
    {
        if (maintenance.Kilometers < 0)
        {
            return new ValidationException("Kilometers cannot be negative");
        }

        if (string.IsNullOrWhiteSpace(maintenance.Description))
        {
            return new ValidationException("Description cannot be empty");
        }

        return null;
    }
}