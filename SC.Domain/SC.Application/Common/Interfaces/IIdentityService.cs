
using SC.Application.Common.ViewModels;

namespace SC.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        string? GetUserId();
        string? GetUserName();
        bool IsAuthenticated();
    }
}
