﻿namespace EFBlog.ViewModels
{
    public class UpdateArticleViewModel
    {
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ArticleContent { get; set; } = string.Empty;
        public bool IsDelete { get; set; }
    }
}