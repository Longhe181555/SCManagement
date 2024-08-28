using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SC.Application.Common.ViewModels;
using SC.Domain.Entities;

namespace SC.Application.Common.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<string?> HandleLoginWithGoogleAsync(TokenViewModel request);
        Task<string?> HandleLoginWithFacebookAsync(TokenViewModel request);
        Task<string?> LoginAsync(LoginViewModel request);
    }
}
