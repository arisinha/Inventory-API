using web_api.Models;
using MySql.Data.MySqlClient;
namespace web_api.Repository;

public class VentasRepository(IConfiguration configuration) : IVentasRepository
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    //OBTENER TODOS
    public async Task<List<Ventas>> GetAllAsync()
    {
        var ventas = new List<Ventas>();

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command =
            new MySqlCommand(
                "SELECT id_venta, id, id_clienteVenta, fecha_venta FROM ventas;",
                connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            ventas.Add(new Ventas
            { 
                IdVenta = (int)reader["id_venta"],
                IdProducto = (int)reader["id"],
                IdCliente = (int)reader["id_clienteVenta"],
                FechaVenta = (DateTime)reader["fecha_venta"],
            });
        }
                
        return ventas;
    }
    
    //OBTENER POR ID
    public async Task<object> GetByIdAsync(int idVenta)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "SELECT id_venta, id, id_clienteVenta, fecha_venta FROM ventas WHERE id_venta = @id_venta;";
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id_venta", idVenta);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Ventas
            {
                IdVenta = (int)reader["id_venta"],
                IdProducto = (int)reader["id"],
                IdCliente = (int)reader["id_clienteVenta"],
                FechaVenta = (DateTime)reader["fecha_venta"],
            };
        }

        return null!;
    }
    
    //AGREGAR VENTA
    public async Task<int> AddAsync(Ventas? ventas)
    {
        if (ventas == null) return 0;

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "INSERT INTO ventas (id_venta, id, id_clienteVenta, fecha_venta) VALUES (@id_venta, @id, @id_clienteVenta, @fecha_venta); SELECT LAST_INSERT_ID();";

        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id_venta", ventas.IdVenta);
        command.Parameters.AddWithValue("@id", ventas.IdProducto);
        command.Parameters.AddWithValue("@id_clienteVenta", ventas.IdCliente);
        command.Parameters.AddWithValue("@fecha_venta", ventas.FechaVenta);
        await command.ExecuteNonQueryAsync();

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }
    
    // Actualizar Venta
    public async Task<bool> UpdateAsync(Ventas? ventas)
    {
        if (ventas == null) return false;

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = @"
            UPDATE ventas 
            SET id = @id, 
                id_clienteVenta = @id_clienteVenta, 
                fecha_venta = @fecha_venta
            WHERE id_venta = @id_venta;";

        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id_venta", ventas.IdVenta);
        command.Parameters.AddWithValue("@id", ventas.IdProducto);
        command.Parameters.AddWithValue("@id_clienteVenta", ventas.IdCliente);
        command.Parameters.AddWithValue("@fecha_venta", ventas.FechaVenta);

        var rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }
    
    //ELIMINAR VENTA
    public async Task<bool> DeleteAsync(int idVenta)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "DELETE FROM ventas WHERE id_venta = @id_venta;";

        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id_venta", idVenta);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}