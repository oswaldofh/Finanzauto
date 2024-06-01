using Finanzauto.Common.Response;
using Finanzauto.Domain.DTOs;
using Finanzauto.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Finanzauto.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();

        Task<User> GetUserAsync(string email);

        Task<User> GetUserDocumentAsync(string document);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<User> AddUserAsync(CreateUserDto model);
        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        //Task<SignInResult> LoginAsync(LoginUserDto model);
        Task<LoginResponse> LoginAsync(LoginUserDto model);
        Task LogoutAsync();
    }
}
