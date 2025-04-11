using web_api.Models;
namespace web_api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task<bool> Save(Product product);
        Task<bool> UpdateProductAsync(Product product);
    }
}