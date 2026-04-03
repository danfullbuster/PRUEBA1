using AgriculturalCrm.Api.Controllers;
using AgriculturalCrm.Api.Models;
using AgriculturalCrm.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AgriculturalCrm.Api.Tests;

public class ClientesControllerTests
{
    [Fact]
    public async Task Crear_cuando_es_valido_devuelve_201_y_cuerpo()
    {
        var esperado = new ClienteResponse
        {
            Id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            Nombre = "Ana",
            Email = "ana@example.com",
            Telefono = "+573001234567",
            NombreFinca = "La Esperanza",
            Hectareas = 12.5m,
            CreadoEn = DateTimeOffset.UtcNow
        };

        var mock = new Mock<IClientesService>();
        mock.Setup(s => s.CrearAsync(It.IsAny<CrearClienteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(esperado);

        var controller = new ClientesController(mock.Object);
        var request = new CrearClienteRequest
        {
            Nombre = "Ana",
            Email = "ana@example.com",
            Telefono = "+573001234567",
            NombreFinca = "La Esperanza",
            Hectareas = 12.5m
        };

        var resultado = await controller.Crear(request, CancellationToken.None);

        var created = Assert.IsType<CreatedResult>(resultado.Result);
        Assert.Equal($"/api/clientes/{esperado.Id}", created.Location);
        var cuerpo = Assert.IsType<ClienteResponse>(created.Value);
        Assert.Equal(esperado.Email, cuerpo.Email);
        mock.Verify(s => s.CrearAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Crear_cuando_modelo_invalido_no_llama_servicio_y_devuelve_400()
    {
        var mock = new Mock<IClientesService>();
        var controller = new ClientesController(mock.Object);
        controller.ModelState.AddModelError("Nombre", "Requerido");

        var resultado = await controller.Crear(new CrearClienteRequest(), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
        mock.Verify(s => s.CrearAsync(It.IsAny<CrearClienteRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Crear_cuando_email_duplicado_devuelve_409()
    {
        var mock = new Mock<IClientesService>();
        mock.Setup(s => s.CrearAsync(It.IsAny<CrearClienteRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Ya existe un cliente con ese email."));

        var controller = new ClientesController(mock.Object);
        var request = new CrearClienteRequest
        {
            Nombre = "Ana",
            Email = "dup@example.com",
            Telefono = "1",
            NombreFinca = "F",
            Hectareas = 1m
        };

        var resultado = await controller.Crear(request, CancellationToken.None);

        Assert.IsType<ConflictObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Listar_devuelve_200_y_lista()
    {
        var lista = new List<ClienteResponse>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Beto",
                Email = "beto@example.com",
                Telefono = "2",
                NombreFinca = "San José",
                Hectareas = 3m,
                CreadoEn = DateTimeOffset.UtcNow
            }
        };

        var mock = new Mock<IClientesService>();
        mock.Setup(s => s.ListarAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(lista);

        var controller = new ClientesController(mock.Object);
        var resultado = await controller.Listar(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(resultado.Result);
        var cuerpo = Assert.IsAssignableFrom<IReadOnlyList<ClienteResponse>>(ok.Value);
        Assert.Single(cuerpo);
    }
}
