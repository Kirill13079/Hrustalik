using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Common.Model
{
    public class Record
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Author Author { get; set; }

        public string Description { get; set; }

        public string TextXAML { get; set; }

        public string Image { get; set; }

        public bool IsNews { get; set; }

        public bool IsHot { get; set; }

        public bool IsArticle { get; set; }

        public bool IsPopular { get; set; }

        public bool IsYoutube { get; set; }

        [NotMapped]
        public bool IsBookmark { get; set; }

        public string Source { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public Category Category { get; set; }
    }
}
