using Finanzauto.Domain.Entities;
using Finanzauto.Domain.Repositories;
using Finanzauto.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Finanzauto.Infrastructure.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DataContext _context;

        public VehicleRepository(DataContext context)
        {
            _context = context;
        }
        public async  Task<bool> Delete(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleAudits)
                .Include(v => v.VehiclePhotos)
                .Include(v => v.Brand)
                .Include(v => v.Phase)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (vehicle == null)
            {
                return false;
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePhoto(string name)
        {

            var photo = await _context.VehiclePhotos
                .FirstOrDefaultAsync(p => p.Image == name);

            if (photo == null)
            {
                return false;
            }

            _context.VehiclePhotos.Remove(photo);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeletePhoto(VehiclePhoto model)
        {

            _context.VehiclePhotos.Remove(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleAudits)
                .Include(v => v.VehiclePhotos)
                .Include(v => v.Brand)
                .Include(v => v.Phase)
                .AnyAsync(p => p.Id == id);
        }

        public async Task<Vehicle> Get(int id)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleAudits)
                .Include(v => v.VehiclePhotos)
                .Include(v => v.Brand)
                .Include(v => v.Phase)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await _context.Vehicles
                .Include(v => v.VehicleAudits)
                .Include(v => v.VehiclePhotos)
                .Include(v => v.Brand)
                .Include(v => v.Phase)
               .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllActive()
        {
            return await _context.Vehicles
                .Include(v => v.VehicleAudits)
                .Include(v => v.VehiclePhotos)
                .Include(v => v.Brand)
                .Include(v => v.Phase)
                .Where(p => p.PhaseId != 4)
               .ToListAsync();

          
        }

        public async Task<Vehicle> GetByName(string name)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleAudits)
                .Include(v => v.VehiclePhotos)
                .Include(v => v.Brand)
                .Include(v => v.Phase)
                .FirstOrDefaultAsync(c => c.Plate.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<VehiclePhoto> GetPhoto(int id)
        {
            return await _context.VehiclePhotos
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<VehiclePhoto>> GetPhotosVehicle(int id)
        {
            return await _context.VehiclePhotos
               .Include(v => v.Vehicle)
               .Where(p => p.VehicleId == id)
               .ToListAsync();
        }

        public async Task Save(Vehicle model)
        {
            _context.Vehicles.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAudit(VehicleAudit model)
        {
            _context.VehicleAudits.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task SavePhoto(VehiclePhoto model)
        {
            _context.VehiclePhotos.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Vehicle model)
        {

            _context.Vehicles.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
