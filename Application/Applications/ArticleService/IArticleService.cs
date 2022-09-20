using Application.Applications.ArticleService.Models;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Applications.ArticleService
{
    public interface IArticleService
    {
        Task<ImageUploadResponse> UploadImage(IFormFile upload);

        Task CreateArticle(string content);

        Task UpdateArticle(UpdateArticleViewModel model);

        Task<IList<Article>> GetArticle(long? id);

        Task<IList<Article>> GetUpdateArticle(long? id);
    }
}