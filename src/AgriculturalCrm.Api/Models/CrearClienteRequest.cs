using System.ComponentModel.DataAnnotations;

namespace AgriculturalCrm.Api.Models;

public class CrearClienteRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [MaxLength(50)]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre de la finca es obligatorio.")]
    [MaxLength(300)]
    public string NombreFinca { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Las hectáreas deben ser mayores a cero.")]
    public decimal Hectareas { get; set; }
}
