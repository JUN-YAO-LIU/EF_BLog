using EFBlog.Applications.ArticleService.Models;
using EFBlog.Models;
using EFBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EFBlog.Applications.ArticleService
{
    public interface IArticleService
    {
        Task<ImageUploadResponse> UploadImage(IFormFile upload);

        Task CreateArticle(string content);

        Task UpdateArticle(UpdateArticleViewModel model);

        Task<IList<Article>> GetArticle(long? id);

        Task<IList<Article>> GetUpdateArticle(long? id);

        Task<IList<Article>> GetMoreArticleList(long id);
    }
}