using AgriculturalCrm.Api.Data;
using AgriculturalCrm.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriculturalCrm.Api.Services;

public class ClientesService : IClientesService
{
    private readonly CrmDbContext _db;

    public ClientesService(CrmDbContext db)
    {
        _db = db;
    }

    public async Task<ClienteResponse> CrearAsync(CrearClienteRequest request, CancellationToken cancellationToken = default)
    {
        var existeEmail = await _db.Clientes.AnyAsync(c => c.Email == request.Email, cancellationToken);
        if (existeEmail)
            throw new InvalidOperationException("Ya existe un cliente con ese email.");

        var entity = new Cliente
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre.Trim(),
            Email = request.Email.Trim(),
            Telefono = request.Telefono.Trim(),
            NombreFinca = request.NombreFinca.Trim(),
            Hectareas = request.Hectareas,
            CreadoEn = DateTimeOffset.UtcNow
        };

        _db.Clientes.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<IReadOnlyList<ClienteResponse>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var items = await _db.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync(cancellationToken);
        return items.ConvertAll(Map);
    }

    private static ClienteResponse Map(Cliente c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        Email = c.Email,
        Telefono = c.Telefono,
        NombreFinca = c.NombreFinca,
        Hectareas = c.Hectareas,
        CreadoEn = c.CreadoEn
    };
}
