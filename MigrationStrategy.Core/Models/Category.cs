namespace MigrationStrategy.Core.Models
{
    public class Category
    {
        private readonly string _name;
        public Category(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be null or empty.", nameof(name));
            _name = name;
        }
        public string GetName() => _name;
    }
}
