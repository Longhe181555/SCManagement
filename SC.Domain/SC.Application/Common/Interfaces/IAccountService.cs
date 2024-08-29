using Microsoft.AspNetCore.Identity;
using SC.Application.Common.ViewModels;

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
