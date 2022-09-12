using EFBlog.DbAccess;
using Microsoft.EntityFrameworkCore;

namespace EFBlog.Applications.ArticleService
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;

        public AuthService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> LoginUserCheckPwd(string code)
        {
            return await _db.AuthUsers.AnyAsync(x => x.Pwd == code);
        }
    }
}