using HealthApp.Common.Model;
using HealthApp.ViewModels.Data;

namespace HealthApp.Extensions
{
    public static class RecordExtension
    {
        public static RecordViewModel ConvertToRecordModel(this Record record)
        {
            return new RecordViewModel
            {
                Id = record.Id,
                Name = record.Name,
                Author = record.Author,
                Description = record.Description,
                TextXAML = record.TextXAML,
                Image = record.Image,
                IsNews = record.IsNews,
                IsHot = record.IsHot,
                IsArticle = record.IsArticle,
                IsPopular = record.IsPopular,
                IsYoutube = record.IsYoutube,
                Source = record.Source,
                DateAdded = record.DateAdded,
                Category = record.Category
            };
        }
    }
}
