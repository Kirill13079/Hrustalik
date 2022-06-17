namespace HealthApp.Common.Model
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected static bool EqualsHelper(Category first, Category second)
        {
            return first.Id == second.Id;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var other = obj as Category;

            if (other == null)
            {
                return false;
            }

            return EqualsHelper(this, other);
        }
    }
}
