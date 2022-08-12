﻿using EFBlog.Applications.ArticleService.Models;
using EFBlog.DbAccess;
using EFBlog.Models;
using EFBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFBlog.Applications.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _db;

        public ArticleService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateArticle(string content)
        {
            var article = new Article
            {
                ArticleContent = content,
                IsDelete = false
            };

            _db.Articles.Add(article);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateArticle(UpdateArticleViewModel model)
        {
            var a = await _db.Articles.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (a is not null)
            {
                a.Title = model.Title;
                a.ArticleContent = model.ArticleContent;
                a.IsDelete = model.IsDelete;

                _db.Articles.Update(a);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IList<Article>> GetArticle(long? id)
        {
            return await _db.Articles
                .Where(x => x.IsDelete == false)
                .ToListAsync();
        }

        public async Task<IList<Article>> GetUpdateArticle(long? id)
        {
            if (id == null)
            {
                return await _db.Articles.ToListAsync();
            }
            else
            {
                return await _db.Articles.Where(x => x.Id == id).ToListAsync();
            }
        }

        public async Task<ImageUploadResponse> UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null!;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var filePath = Path.Combine(
                   Directory.GetCurrentDirectory(), "wwwroot/images",
                   fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                //程式寫入的本地資料夾裡面
                await upload.CopyToAsync(stream);
            }

            var url = $"{"/images/"}{fileName}";

            var reslult = new ImageUploadResponse
            {
                Uploaded = 1,
                FileName = fileName,
                Url = url,
                Msg = "sucess",
            };

            return reslult;
        }
    }
}