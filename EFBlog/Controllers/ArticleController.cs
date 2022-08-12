using EFBlog.Applications.ArticleService;
using EFBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EFBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _article;

        public ArticleController(IArticleService articleService)
        {
            _article = articleService;
        }

        [HttpGet("Article/{id}")]
        public async Task<IActionResult> Index(long id)
        {
            var model = await _article.GetArticle(id);
            var result = new ArticleViewModel();

            if (model is not null && model.Count > 0)
            {
                result = model.Select(x => new ArticleViewModel
                {
                    Id = x.Id,
                    ArticleContent = x.ArticleContent,
                    Title = x.Title
                }).First();

                return View(result);
            }

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult CreateArticle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArticle(string Content)
        {
            await _article.CreateArticle(Content);
            return Redirect("/");
        }

        [HttpGet("GetArticleList")]
        public async Task<IActionResult> GetArticleList()
        {
            var model = await _article.GetUpdateArticle(null);
            var result = new List<UpdateArticleViewModel>();

            if (model is not null && model.Count > 0)
            {
                result = model.Select(x => new UpdateArticleViewModel
                {
                    Id = x.Id,
                    ArticleContent = x.ArticleContent,
                    Title = x.Title,
                    IsDelete = x.IsDelete
                }).ToList();
            }

            return View(result);
        }

        [HttpGet("GetArticle/{id}")]
        public async Task<IActionResult> GetArticle(long id)
        {
            var model = await _article.GetUpdateArticle(id);
            var result = new UpdateArticleViewModel();

            if (model is not null && model.Count > 0)
            {
                result = model.Select(x => new UpdateArticleViewModel
                {
                    Id = x.Id,
                    ArticleContent = x.ArticleContent,
                    Title = x.Title,
                    IsDelete = x.IsDelete
                }).First();

                return View(result);
            }

            return Redirect("/");
        }

        // 細節 get post 的function name 相同 可能會出現無法post 的情況 錯誤405
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArticle(UpdateArticleViewModel model)
        {
            await _article.UpdateArticle(model);
            return RedirectToAction($"GetArticleList");
        }

        // 前端欄位名稱已經固定
        public async Task<IActionResult> Uploads(IFormFile upload)
        {
            var obj = await _article.UploadImage(upload);

            return Json(new
            {
                uploaded = obj.Uploaded,
                fileName = obj.FileName,
                url = obj.Url,
                error = new
                {
                    message = obj.Msg
                }
            });
        }
    }
}