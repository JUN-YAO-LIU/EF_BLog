using Application.Services.ArticleService.Models;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;

namespace Application.Services.ArticleService
{
    public class LocalArticleService
    {
        public LocalArticleService()
        {
        }

        public async Task CreateArticle(string content)
        {
            int i = 1;
            var filePath = Path.Combine(
                   Directory.GetCurrentDirectory(), "wwwroot/images",
                   i.ToString() + ".txt");

            while (File.Exists(filePath))
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", i.ToString() + ".txt");
                i++;
            }

            File.WriteAllText(filePath, content);
        }

        public async Task UpdateArticle(UpdateArticleViewModel model)
        {
        }

        public async Task<IList<Article>> GetArticle(long? id)
        {
            return null;
        }

        public async Task<IList<Article>> VagueSearchAsync(string id)
        {
            return null;
        }

        public async Task<IList<Article>> GetUpdateArticle(long? id)
        {
            return null;
        }

        public async Task<IList<Article>> GetMoreArticleList(long id)
        {
            return null;
        }

        public async Task<ImageUploadResponse> UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null!;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var filePath = Path.Combine(
                   Directory.GetCurrentDirectory(), "wwwroot/images",
                   fileName);

            using (var stream = File.Create(filePath))
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

        private void GetInsertContentImages(string content)
        {
            var strList = content.Split("images/").ToList();
            IList<Guid> imageList = new List<Guid>();

            foreach (var str in strList)
            {
                if (str.Length < 36)
                {
                    continue;
                }

                var tempGuid = str.Substring(0, 36);
                if (Guid.TryParse(tempGuid, out Guid r))
                {
                    imageList.Add(r);
                }
            }
        }
    }
}