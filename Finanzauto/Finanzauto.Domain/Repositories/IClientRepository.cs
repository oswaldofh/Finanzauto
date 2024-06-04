using Finanzauto.Domain.Entities;

namespace Finanzauto.Domain.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAll();
        Task<Client> Get(int id);
        Task<Client> GetName(string documento);
        Task Save(Client model);
        Task Update(Client model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);
    }
}
