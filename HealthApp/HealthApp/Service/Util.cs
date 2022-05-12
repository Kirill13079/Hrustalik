using HealthApp.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthApp.Service
{
    public class Util
    {
        public Util()
        {
            Bookmarks = new List<Bookmark>();
        }

        public List<Bookmark> Bookmarks { get; set; }

        public Customer Customer { get; set; }
    }
}
