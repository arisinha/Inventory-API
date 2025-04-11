using web_api.Models;

namespace web_api.Repository;

public interface IClientesRepository
{
        Task<List<Clientes>> GetAllAsync();
        Task<object> GetByIdAsync(int idCliente);
        Task<int> AddAsync(Clientes? clientes);
        Task<bool> UpdateAsync(Clientes clientes);
        Task<bool> DeleteAsync(int idCliente);
  
}
