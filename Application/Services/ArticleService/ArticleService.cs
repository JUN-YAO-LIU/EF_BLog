using Application.Services.ArticleService.Models;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;

namespace Application.Services.ArticleService
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
                Title = DateTime.UtcNow.ToUniversalTime().ToString(),
                ArticleContent = content,
                IsDelete = false
            };

            _db.Articles.Add(article);
            await _db.SaveChangesAsync();

            int i = 1;
            var filePath = Path.Combine(
                   Directory.GetCurrentDirectory(), "wwwroot/articles",
                   i.ToString() + ".txt");

            while (File.Exists(filePath))
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/articles", i.ToString() + ".txt");
                i++;
            }

            File.WriteAllText(filePath, content);
        }

        public async Task UpdateArticle(UpdateArticleViewModel model)
        {
            GetInsertContentImages(model.ArticleContent);

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
            //string path = Path.Combine(
            //       Directory.GetCurrentDirectory(), "wwwroot/articles",
            //       i.ToString() + ".txt");

            // Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var dataList = new List<Article>();

            var filePaths = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/articles"));

            if (filePaths is not null && filePaths.Count() > 0)
            {
                foreach (var file in filePaths)
                {
                    string readText = File.ReadAllText(file);

                    dataList.Add(new Article
                    {
                        ArticleContent = readText,
                        Title = "1",
                    });
                }

                return dataList;
            }

            //string readText = File.ReadAllText(path);
            if (id == null)
            {
                return await _db.Articles
                .Where(x => x.IsDelete == false)
                .OrderByDescending(x => x.Id)
                .Take(2)
                .ToListAsync();
            }
            else
            {
                return await _db.Articles
                .Where(x => x.IsDelete == false && x.Id == id)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
            }
        }

        public async Task<IList<Article>> VagueSearchAsync(string id)
        {
            return await _db.Articles
            .Where(x => x.IsDelete == false && x.Title.Contains(id))
            .OrderByDescending(x => x.Id)
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

        public async Task<IList<Article>> GetMoreArticleList(long id)
        {
            return await _db.Articles
                .Where(x => x.Id < id)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<ImageUploadResponse> UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null!;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var filePath = Path.Combine(
                   Directory.GetCurrentDirectory(), "wwwroot/images",
                   fileName);

            //var tempImg = Image.FromStream(upload.OpenReadStream());

            ////計算大小
            //int imgH = 0, imgW = 700;

            //imgH = (700 * tempImg.Height) / tempImg.Width;

            //using var image = new Bitmap(tempImg, new Size(imgW, imgH));

            //image.Save(filePath, ImageFormat.Jpeg);

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