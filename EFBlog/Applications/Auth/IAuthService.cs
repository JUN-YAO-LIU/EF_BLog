using System.Security.Claims;

namespace EFBlog.Applications.ArticleService
{
    public interface IAuthService
    {
        Task<bool> LoginUserCheckPwd(string code);
    }
}