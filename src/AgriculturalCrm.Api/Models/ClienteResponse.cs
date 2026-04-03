namespace AgriculturalCrm.Api.Models;

public class ClienteResponse
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string NombreFinca { get; set; } = string.Empty;
    public decimal Hectareas { get; set; }
    public DateTimeOffset CreadoEn { get; set; }
}
