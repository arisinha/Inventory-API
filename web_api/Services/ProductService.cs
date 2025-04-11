using web_api.Models;
using web_api.Repository;

namespace web_api.Services
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        public async Task<List<Product>> GetAll()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public Task<bool> Save(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            return await repository.UpdateAsync(product);
        }
    }
}