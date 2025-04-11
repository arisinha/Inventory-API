namespace web_api.Models;

public class Ventas
{
    public int IdVenta { get; set; }
    public int IdCliente { get; set; }
    public int IdProducto { get; set; }
    public DateTime FechaVenta { get; set; }
}