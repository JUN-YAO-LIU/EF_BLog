using EFBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;

namespace EFBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateArticle()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Article()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostContent(string Content)
        {
            TempData["Content"] = Content;
            return RedirectToAction("Article");
        }

        // 前端欄位名稱已經固定
        public async Task<IActionResult> Uploads(IFormFile upload)
        {
            if (upload.Length <= 0) return null;

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

            return Json(new
            {
                uploaded = 1,
                fileName = fileName,
                url = url,
                error = new
                {
                    message = "success"
                }
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}