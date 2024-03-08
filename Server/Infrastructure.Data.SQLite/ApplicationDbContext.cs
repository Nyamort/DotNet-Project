using Microsoft.EntityFrameworkCore;
using Server.Domain.Entity;

namespace Server.Infrastructure.Data.SQLite;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Vehicle> Vehicles { get; set; }
    public required DbSet<Maintenance> Maintenances { get; set; }
    public required DbSet<Model> Models { get; set; }
}