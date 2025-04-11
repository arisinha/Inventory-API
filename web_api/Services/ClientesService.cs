using web_api.Models;
using web_api.Repository;

namespace web_api.Services
{
    public class ClientesService(IClientesRepository clientesRepository) : IClientesService
    {
        // Agregar un nuevo cliente
        public Task<List<Clientes>> GetAllAsync()
        {
            return clientesRepository.GetAllAsync();
        }

        public Task<object> GetByIdAsync(int idCliente)
        {
            return clientesRepository.GetByIdAsync(idCliente);
        }

        public async Task<int> AddAsync(Clientes? cliente)
        {
            return await clientesRepository.AddAsync(cliente);
        }

        // Actualizar cliente
        public async Task<bool> UpdateAsync(Clientes cliente)
        {
            return await clientesRepository.UpdateAsync(cliente);
        }

        // Eliminar cliente
        public async Task<bool> DeleteAsync(int idCliente)
        {
            return await clientesRepository.DeleteAsync(idCliente);
        }
    }
}