using EFBlog.Applications.ArticleService;
using EFBlog.DbAccess;
using EFBlog.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
        //options => options.UseSqlServer("Server=(localdb)\\MSSqlLocalDb;Database=Blog;"));
        options => options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Blog;Trusted_Connection=True;"));

// Add services to the container.
builder.Services.AddControllersWithViews();

// ���U�Ȼs�Ƥ���
builder.Services.AddTransient<IArticleService, ArticleService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();