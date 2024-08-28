using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SC.Application.Common.Interfaces;
using SC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser?> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<IdentityResult> CreateUserAsync(IdentityUser user)
        {
            return await _userManager.CreateAsync(user);
        }
        public async Task<IdentityResult> UpdateUserAsync(IdentityUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<IdentityUser?> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
