namespace MigrationStrategy.Core.Models
{
    public class Product
    {
        private readonly string _name;
        public Product(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            _name = name;
        }
        public string GetName() => _name;
    }
}
