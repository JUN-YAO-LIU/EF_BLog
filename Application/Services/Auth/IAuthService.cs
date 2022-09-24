using System.Security.Claims;

namespace Application.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginUserCheckPwd(string code);
    }
}