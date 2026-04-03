using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AgriculturalCrm.Api.Data;

/// <summary>
/// Solo para diseño (dotnet ef migrations). No se usa en tiempo de ejecución.
/// </summary>
public class CrmDbContextFactory : IDesignTimeDbContextFactory<CrmDbContext>
{
    public CrmDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("CRM_DESIGN_CONNECTION")
            ?? "Server=(localdb)\\mssqllocaldb;Database=AgriculturalCrmDesign;Trusted_Connection=True;TrustServerCertificate=True;";
        var options = new DbContextOptionsBuilder<CrmDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        return new CrmDbContext(options);
    }
}
