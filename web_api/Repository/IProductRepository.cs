using web_api.Models;

namespace web_api.Repository;

public interface IProductRepository{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<int> AddAsync(Product? product);
    Task<bool> UpdateAsync(Product? product);
    Task<bool> DeleteAsync(int id);
}