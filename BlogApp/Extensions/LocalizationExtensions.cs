using BlogApp.Models;
using System.Globalization;

namespace BlogApp.Extensions
{
    public static class LocalizationExtensions
    {
        private static bool IsEnglish => CultureInfo.CurrentCulture.Name == "en";

        public static string LocalizedTitle(this Post post)
        {
            if (IsEnglish && !string.IsNullOrWhiteSpace(post.TitleEn))
                return post.TitleEn;
            return post.Title;
        }

        public static string LocalizedContent(this Post post)
        {
            if (IsEnglish && !string.IsNullOrWhiteSpace(post.ContentEn))
                return post.ContentEn;
            return post.Content;
        }

        public static string LocalizedSummary(this Post post)
        {
            if (IsEnglish && !string.IsNullOrWhiteSpace(post.SummaryEn))
                return post.SummaryEn;
            return post.Summary;
        }

        public static string LocalizedName(this Category category)
        {
            if (category == null) return null;
            if (IsEnglish && !string.IsNullOrWhiteSpace(category.NameEn))
                return category.NameEn;
            return category.Name;
        }

        public static string LocalizedContent(this Comment comment)
        {
            if (IsEnglish && !string.IsNullOrWhiteSpace(comment.ContentEn))
                return comment.ContentEn;
            return comment.Content;
        }
    }
}