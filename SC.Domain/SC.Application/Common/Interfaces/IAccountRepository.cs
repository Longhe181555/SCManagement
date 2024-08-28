using Microsoft.AspNetCore.Identity;
using SC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
