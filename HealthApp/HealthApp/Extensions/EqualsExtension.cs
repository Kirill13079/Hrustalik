using System.Collections.Generic;

namespace HealthApp.Extensions
{
    public static class EqualsExtension
    {
        public static bool EqualsHelper<T>(this List<T> items, object obj)
        {
            foreach (var item in items)
            {
                if (item.Equals(obj))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
