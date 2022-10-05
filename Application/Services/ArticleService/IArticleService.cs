using Application.Services.ArticleService.Models;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.ArticleService
{
    public interface IArticleService
    {
        Task<ImageUploadResponse> UploadImage(IFormFile upload);

        Task CreateArticle(string content);

        Task UpdateArticle(UpdateArticleViewModel model);

        Task<IList<Article>> GetArticle(long? id);

        Task<IList<Article>> GetUpdateArticle(long? id);

        Task<IList<Article>> GetMoreArticleList(long id);

        Task<IList<Article>> VagueSearchAsync(string id);
    }
}