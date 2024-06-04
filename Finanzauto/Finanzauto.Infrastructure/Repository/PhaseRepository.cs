using Finanzauto.Domain.Entities;
using Finanzauto.Domain.Repositories;
using Finanzauto.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Finanzauto.Infrastructure.Repository
{
    public class PhaseRepository : IPhaseRepository
    {
        private readonly DataContext _context;

        public PhaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var city = await _context.Phases.FirstOrDefaultAsync(s => s.Id == id);

            if (city == null)
            {
                return false;
            }

            _context.Phases.Remove(city);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Phases.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Phase>> GetAll()
        {
            return await _context.Phases
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Phase> Get(int id)
        {
            return await _context.Phases.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Phase> GetName(string name)
        {
            return await _context.Phases.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task Save(Phase model)
        {
            _context.Phases.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Phase model)
        {
            _context.Phases.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
