using Microsoft.AspNetCore.Identity;

namespace SC.Application.Common.Interfaces
{
    public interface IAccountRepository
    {
        Task<IdentityUser?> FindByUsernameAsync(string username);
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
        Task<IdentityResult> CreateUserAsync(IdentityUser user);
        Task<IdentityResult> UpdateUserAsync(IdentityUser user);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
        Task<IdentityUser?> FindByIdAsync(string userId);
    }
}
