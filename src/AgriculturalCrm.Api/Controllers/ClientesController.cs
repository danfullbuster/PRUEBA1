using AgriculturalCrm.Api.Models;
using AgriculturalCrm.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgriculturalCrm.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClientesService _clientesService;

    public ClientesController(IClientesService clientesService)
    {
        _clientesService = clientesService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClienteResponse>> Crear(
        [FromBody] CrearClienteRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var creado = await _clientesService.CrearAsync(request, cancellationToken);
            return Created($"/api/clientes/{creado.Id}", creado);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClienteResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ClienteResponse>>> Listar(CancellationToken cancellationToken)
    {
        var lista = await _clientesService.ListarAsync(cancellationToken);
        return Ok(lista);
    }
}
