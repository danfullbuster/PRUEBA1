using AgriculturalCrm.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriculturalCrm.Api.Data;

public class CrmDbContext : DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(200);
            e.Property(x => x.Email).IsRequired().HasMaxLength(320);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Telefono).IsRequired().HasMaxLength(50);
            e.Property(x => x.NombreFinca).IsRequired().HasMaxLength(300);
            e.Property(x => x.Hectareas).HasPrecision(18, 2);
        });
    }
}
