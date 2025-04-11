using MySql.Data.MySqlClient;
using web_api.Models;

namespace web_api.Repository;

public class ClientesRepository(string connectionString) : IClientesRepository
{
    public async Task<List<Clientes>> GetAllAsync()
    {
        var clientes = new List<Clientes>();

        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        await using var command =
            new MySqlCommand(
                "SELECT id_cliente, nombre_cliente, apellido_paterno, apellido_materno, correo_cliente FROM clientes;",
                connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            clientes.Add(new Clientes
            {
                IdCliente = (int)reader["id_cliente"],
                NombreCliente = reader["nombre_cliente"].ToString(),
                ApellidoPaterno = reader["apellido_paterno"].ToString(),
                ApellidoMaterno = reader["apellido_materno"].ToString(),
                CorreoCliente = reader["correo_cliente"].ToString()
            });
        }

        return clientes;
    }

    public async Task<object> GetByIdAsync(int idCliente)
    {
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        const string query = "SELECT id_cliente, nombre_cliente, apellido_paterno, apellido_materno, correo_cliente FROM clientes WHERE id_cliente = @id_cliente;";
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id_cliente", idCliente);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Clientes
            {
                IdCliente = (int)reader["id_cliente"],
                NombreCliente = reader["nombre_cliente"].ToString(),
                ApellidoPaterno = reader["apellido_paterno"].ToString(),
                ApellidoMaterno = reader["apellido_materno"].ToString(),
                CorreoCliente = reader["correo_cliente"].ToString()
            };
        }

        return null!;
    }

    public async Task<int> AddAsync(Clientes? clientes)
    {
        if (clientes == null) return 0;

        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        const string query = "INSERT INTO clientes (nombre_cliente, apellido_paterno, apellido_materno, correo_cliente) VALUES (@nombre_cliente, @apellido_paterno, @apellido_materno, @correo_cliente); SELECT LAST_INSERT_ID();";

        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@nombre_cliente", clientes.NombreCliente);
        command.Parameters.AddWithValue("@apellido_paterno", clientes.ApellidoPaterno);
        command.Parameters.AddWithValue("@apellido_materno", clientes.ApellidoMaterno);
        command.Parameters.AddWithValue("@correo_cliente", clientes.CorreoCliente);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task<bool> UpdateAsync(Clientes clientes)
    {
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        const string query = "UPDATE clientes SET nombre_cliente = @nombre_cliente, apellido_paterno = @apellido_paterno, apellido_materno = @apellido_materno, correo_cliente = @correo_cliente WHERE id_cliente = @id_cliente;";

        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id_cliente", clientes.IdCliente);
        command.Parameters.AddWithValue("@nombre_cliente", clientes.NombreCliente);
        command.Parameters.AddWithValue("@apellido_paterno", clientes.ApellidoPaterno);
        command.Parameters.AddWithValue("@apellido_materno", clientes.ApellidoMaterno);
        command.Parameters.AddWithValue("@correo_cliente", clientes.CorreoCliente);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int idCliente)
    {
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            // 1. Eliminar los registros de ventas asociados
            const string deleteVentasQuery = "DELETE FROM ventas WHERE id_clienteVenta = @id_cliente;";
            await using var deleteVentasCommand = new MySqlCommand(deleteVentasQuery, connection, transaction);
            deleteVentasCommand.Parameters.AddWithValue("@id_cliente", idCliente);
            await deleteVentasCommand.ExecuteNonQueryAsync();

            // 2. Eliminar el cliente
            const string deleteClienteQuery = "DELETE FROM clientes WHERE id_cliente = @id_cliente;";
            await using var deleteClienteCommand = new MySqlCommand(deleteClienteQuery, connection, transaction);
            deleteClienteCommand.Parameters.AddWithValue("@id_cliente", idCliente);
            var clienteRowsAffected = await deleteClienteCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return clienteRowsAffected > 0;
        }
        catch (MySqlException ex)
        {
            await transaction.RollbackAsync();
            
            Console.WriteLine($"Error al eliminar el cliente: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error inesperado al eliminar el cliente: {ex.Message}");
            return false;
        }
    }
}

