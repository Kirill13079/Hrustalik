namespace HealthApp.Common.Model
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string Logo { get; set; }

        public string Color { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected static bool EqualsHelper(Author first, Author second)
        {
            return first.Id == second.Id;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var other = obj as Author;

            if (other == null)
            {
                return false;
            }

            return EqualsHelper(this, other);
        }
    }
}
