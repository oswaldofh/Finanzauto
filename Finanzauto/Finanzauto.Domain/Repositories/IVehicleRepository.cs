using Finanzauto.Domain.Entities;

namespace Finanzauto.Domain.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAll();
        Task<IEnumerable<Vehicle>> GetAllActive();
        Task<Vehicle> Get(int id);
        Task<Vehicle> GetByName(string name);
        Task Save(Vehicle model);
        Task Update(Vehicle model);
        Task<bool> Delete(int id);
        Task<bool> Exist(int id);

        Task<VehiclePhoto> GetPhoto(int id);
        Task SavePhoto(VehiclePhoto model);
        Task<bool> DeletePhoto(string name);
        Task<bool> DeletePhoto(VehiclePhoto model);
        Task<IEnumerable<VehiclePhoto>> GetPhotosVehicle(int id);
        Task SaveAudit(VehicleAudit model);
    }
}
