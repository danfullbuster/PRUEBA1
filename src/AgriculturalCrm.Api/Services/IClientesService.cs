using AgriculturalCrm.Api.Models;

namespace AgriculturalCrm.Api.Services;

public interface IClientesService
{
    Task<ClienteResponse> CrearAsync(CrearClienteRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ClienteResponse>> ListarAsync(CancellationToken cancellationToken = default);
}
