using Finanzauto.Domain.Entities;
using Finanzauto.Domain.Repositories;
using Finanzauto.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Finanzauto.Infrastructure.Repository
{
    public class ClientRepository : IClientRepository
    {

        private readonly DataContext _context;

        public ClientRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {

            var data = await _context.Clients.FirstOrDefaultAsync(s => s.Id == id);

            if (data == null)
            {
                return false;
            }

            _context.Clients.Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Clients.AnyAsync(c => c.Id == id);
        }

        public async Task<Client> Get(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _context.Clients
                .Include(v =>v.Vehicle)
                .OrderBy(c => c.Id)
                .ToListAsync();
        }

        public async Task<Client> GetName(string documento)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Document.ToLower().Trim() == documento.ToLower().Trim());
        }

        public async Task Save(Client model)
        {
            _context.Clients.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Client model)
        {
            _context.Clients.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
