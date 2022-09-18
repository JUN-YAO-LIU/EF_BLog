﻿namespace EFBlog.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // 1.可以存入資料庫
                // 2.或是輸出成檔案
                await context.Response
                    .WriteAsync($"{GetType().Name} catch exception. Message: {ex.Message}");
            }
        }
    }
}