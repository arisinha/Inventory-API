namespace web_api.Models;

public class Clientes
{
    public int IdCliente { get; init; }
    public string? NombreCliente { get; init; }
    public string? ApellidoPaterno { get; init; }
    public string? ApellidoMaterno { get; init; }
    public string? CorreoCliente { get; init; }
}