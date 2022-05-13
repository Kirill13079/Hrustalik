using HealthApp.Common.Model;
using System.Collections.Generic;

namespace HealthApp.Service
{
    public class Util
    {
        public List<Bookmark> Bookmarks { get; set; }

        public Customer Customer { get; set; }

        public Util()
        {
            Bookmarks = new List<Bookmark>();
        }
    }
}
