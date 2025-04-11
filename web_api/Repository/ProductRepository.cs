using MySql.Data.MySqlClient;
using web_api.Models;

namespace web_api.Repository;

public class ProductRepository(IConfiguration configuration) : IProductRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    // Insertar en database
    public async Task<int> AddAsync(Product? product)
    {
        if (product == null) return 0;

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query =
            "INSERT INTO products (productName, price) VALUES (@name, @price); SELECT LAST_INSERT_ID();";

        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", product.Name);
        command.Parameters.AddWithValue("@price", product.Price);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    // get products
    public async Task<List<Product>> GetAllAsync()
    {
        var products = new List<Product>();

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new MySqlCommand("SELECT id, productName, price FROM products;", connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Id = (int)reader["id"],
                Name = reader["productName"].ToString(),
                Price = Convert.ToDouble(reader["price"]),
            });
        }

        return products;
    }

    // get product por ID
    public async Task<Product?> GetByIdAsync(int id)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command =
            new MySqlCommand("SELECT id, productName, price FROM products WHERE id = @id;", connection);
        command.Parameters.AddWithValue("@id", id);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Product
            {
                Id = (int)reader["id"],
                Name = reader["productName"].ToString(),
                Price = Convert.ToDouble(reader["price"]),
            };
        }

        return null;
    }

    // Actualizar un producto
    public async Task<bool> UpdateAsync(Product? product)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query =
            "UPDATE products SET productName = @name, price = @price WHERE id = @id;";

        await using var command = new MySqlCommand(query, connection);
        if (product != null)
        {
            command.Parameters.AddWithValue("@id", product.Id);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@price", product.Price);
        }

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }


    // Eliminar un producto
    public async Task<bool> DeleteAsync(int id)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "DELETE FROM products WHERE id = @id;";
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

}