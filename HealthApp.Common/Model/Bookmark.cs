using System.ComponentModel.DataAnnotations;

namespace HealthApp.Common.Model
{
    public class Bookmark
    {
        public int Id { get; set; }

        [Required]
        public Record Record { get; set; }

        public Customer Customer { get; set; }
    }
}
