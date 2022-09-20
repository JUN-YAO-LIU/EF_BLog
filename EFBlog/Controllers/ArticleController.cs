using Application.Applications.ArticleService;
using Application.Applications.ArticleService.Models;
using EFBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("CreateArticle")]
        public IActionResult CreateArticle()
        {
            return View();
        }

        [Authorize]
        [HttpPost("CreateArticle")]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArticle(UpdateArticleViewModel model)
        {
            await _article.UpdateArticle(model);
            return RedirectToAction($"GetArticleList");
        }

        // 前端欄位名稱已經固定
        [Authorize]
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

        [HttpPost]
        public async Task<IActionResult> GetMoreArticle()
        {
            var nextArticleId = TempData["ArticleLastId"];

            if (TempData["ArticleLastId"] != null && int.TryParse(TempData["ArticleLastId"]!.ToString(), out var i))
            {
                var model = await _article.GetMoreArticleList(i);
                var result = new List<UpdateArticleViewModel>();

                if (model is not null && model.Count > 0)
                {
                    result = model.Select(x => new UpdateArticleViewModel
                    {
                        Id = x.Id,
                        ArticleContent = x.ArticleContent,
                        Title = x.Title,
                        IsDelete = x.IsDelete
                    }).Take(1).ToList();

                    TempData["ArticleLastId"] = result.Last().Id.ToString();
                }

                return Json(result);
            }

            return Json(string.Empty);
        }

        [HttpPost]
        public async Task<IActionResult> GetMoreArticleToPartialView()
        {
            var nextArticleId = TempData["ArticleLastId"];

            if (TempData["ArticleLastId"] != null && int.TryParse(TempData["ArticleLastId"]!.ToString(), out var i))
            {
                var model = await _article.GetMoreArticleList(i);
                var result = new List<UpdateArticleViewModel>();

                if (model is not null && model.Count > 0)
                {
                    result = model.Select(x => new UpdateArticleViewModel
                    {
                        Id = x.Id,
                        ArticleContent = x.ArticleContent,
                        Title = x.Title,
                        IsDelete = x.IsDelete
                    }).Take(1).ToList();

                    TempData["ArticleLastId"] = result.Last().Id.ToString();
                }

                return PartialView("/Views/Article/_ArticlePartialView.cshtml", result);
            }

            return PartialView(null);
        }
    }
}