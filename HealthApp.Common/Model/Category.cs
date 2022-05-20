using System;

namespace HealthApp.Common.Model
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public T DeserializeObject<T>(string parameter)
        {
            throw new NotImplementedException();
        }
    }
}
