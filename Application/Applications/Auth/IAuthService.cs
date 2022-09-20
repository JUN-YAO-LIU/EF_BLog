using System.Security.Claims;

namespace Application.Applications.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginUserCheckPwd(string code);
    }
}