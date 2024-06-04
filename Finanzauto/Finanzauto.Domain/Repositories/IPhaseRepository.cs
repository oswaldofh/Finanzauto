using Finanzauto.Domain.Entities;

namespace Finanzauto.Domain.Repositories
{
    public interface IPhaseRepository
    {
        Task<IEnumerable<Phase>> GetAll();
        Task<Phase> Get(int id);
        Task<Phase> GetName(string name);
        Task Save(Phase model);
        Task Update(Phase model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);
    }
}
