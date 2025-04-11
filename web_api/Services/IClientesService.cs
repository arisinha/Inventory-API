using web_api.Models;

namespace web_api.Services
{
    public interface IClientesService
    {
        Task<List<Clientes>> GetAllAsync(); 
        Task<object> GetByIdAsync(int idCliente);
        Task<int> AddAsync(Clientes? cliente);
        Task<bool> UpdateAsync(Clientes cliente);
        Task<bool> DeleteAsync(int idCliente);
    }
}