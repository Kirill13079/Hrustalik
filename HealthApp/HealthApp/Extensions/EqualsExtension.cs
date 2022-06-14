using HealthApp.Common.Model;
using System.Collections.Generic;

namespace HealthApp.Extensions
{
    public static class EqualsExtension
    {
        public static bool EqualsHelper(this List<Author> authors, object obj)
        {
            foreach (var author in authors)
            {
                if (author.Equals(obj))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
