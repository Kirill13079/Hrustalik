using System;

namespace HealthApp.Common.Model
{
    public class Record
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Publication { get; set; }

        public string Description { get; set; }

        public string TextXAML { get; set; }

        public string Image { get; set; }

        public bool SmallSize { get; set; }

        public bool IsNews { get; set; }

        public bool IsHot { get; set; }

        public bool IsArticle { get; set; }

        public bool IsPopular { get; set; }

        public bool IsYoutube { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public Category Category { get; set; }
    }
}
