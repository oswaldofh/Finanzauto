using Finanzauto.Domain.Entities;
using Finanzauto.Domain.Repositories;
using Finanzauto.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Finanzauto.Infrastructure.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly DataContext _context;

        public BrandRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            var data = await _context.Brands.FirstOrDefaultAsync(s => s.Id == id);

            if (data == null)
            {
                return false;
            }

            _context.Brands.Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Brands.AnyAsync(c => c.Id == id);
        }

        public async Task<Brand> Get(int id)
        {
            return await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Brand>> GetAll()
        {

            return await _context.Brands
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Brand> GetName(string name)
        {
            return await _context.Brands.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task Save(Brand model)
        {
            _context.Brands.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Brand model)
        {

            _context.Brands.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
