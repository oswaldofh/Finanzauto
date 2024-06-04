using Finanzauto.Common.Enums;
using Finanzauto.Domain.Entities;
using Finanzauto.Domain.Repositories;

namespace Finanzauto.Infrastructure.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public SeedDb(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            
            await CheckBrandsAsync();
            await CheckPhasesAsync();
            await CheckRolesAsycn();
            await CheckUserAsync("10101010", "Oswaldo", "Fuentes", "oswaldo@yopmail.com",  UserType.Admin);
            await CheckUserAsync("20202020", "Antonio", "Hernandez", "antonio@yopmail.com", UserType.User);

        }

        private async Task CheckUserAsync(string document, string firstName, string lastName, string email,  UserType userType)
        {
            User user = await _userRepository.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Document = document,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = email,
                    UserType = userType
                };

                await _userRepository.AddUserAsync(user, "123456");
                await _userRepository.AddUserToRoleAsync(user, userType.ToString());
            }
        }

        private async Task CheckRolesAsycn()
        {
            await _userRepository.CheckRoleAsync(UserType.Admin.ToString());
            await _userRepository.CheckRoleAsync(UserType.User.ToString());
        }

       

        private async Task CheckBrandsAsync()
        {
            if (!_context.Brands.Any())
            {
                _context.Brands.Add(new Brand { Name = "RENAULT" });
                _context.Brands.Add(new Brand { Name = "MAZDA" });
                _context.Brands.Add(new Brand { Name = "TOYOTA" });
                _context.Brands.Add(new Brand { Name = "KIA" });
                await _context.SaveChangesAsync();

            }
        }

        private async Task CheckPhasesAsync()
        {
            if (!_context.Phases.Any())
            {
                _context.Phases.Add(new Phase { Name = "Disponible" });
                _context.Phases.Add(new Phase { Name = "Reparacion" });
                _context.Phases.Add(new Phase { Name = "En vitrina" });
                _context.Phases.Add(new Phase { Name = "Vendido" });
                await _context.SaveChangesAsync();

            }
        }

    }
}
