using web_api.Models;

namespace web_api.Repository;

public interface IVentasRepository
{
    Task<List<Ventas>> GetAllAsync();
    Task<object> GetByIdAsync(int idVenta);
    Task<int> AddAsync(Ventas? ventas);
    Task<bool> UpdateAsync(Ventas? ventas);
    Task<bool> DeleteAsync(int idVenta);
}