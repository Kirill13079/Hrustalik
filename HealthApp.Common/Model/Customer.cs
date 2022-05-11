using System.Collections.Generic;

namespace HealthApp.Common.Model
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public IList<Bookmark> Bookmarks { get; set; }
    }
}
