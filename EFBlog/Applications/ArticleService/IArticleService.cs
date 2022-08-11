using EFBlog.Applications.ArticleService.Models;
using EFBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace EFBlog.Applications.ArticleService
{
    public interface IArticleService
    {
        Task<ImageUploadResponse> UploadImage(IFormFile upload);

        Task CreateArticle(string content);

        Task<IList<Article>> GetArticle(long? id);
    }
}