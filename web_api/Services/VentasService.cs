using web_api.Models;
using web_api.Repository;

namespace web_api.Services;

public class VentasService(IVentasRepository ventasRepository) : IVentasService
{
    public Task<List<Ventas>> GetAllAsync()
    {
        return ventasRepository.GetAllAsync();
    }

    public Task<object> GetByIdAsync(int idVenta)
    {
        return ventasRepository.GetByIdAsync(idVenta);
    }

    public Task<int> AddAsync(Ventas? ventas)
    {
        return ventasRepository.AddAsync(ventas);
    }

    public Task<bool> UpdateAsync(Ventas? ventas)
    {
        return ventasRepository.UpdateAsync(ventas);
    }

    public Task<bool> DeleteAsync(int idVenta)
    {
        return ventasRepository.DeleteAsync(idVenta);
    }
}