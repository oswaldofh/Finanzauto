using Finanzauto.Domain.Entities;

namespace Finanzauto.Domain.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAll();
        Task<Brand> Get(int id);
        Task<Brand> GetName(string name);
        Task Save(Brand model);
        Task Update(Brand model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);
    }
}
