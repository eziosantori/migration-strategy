namespace MigrationStrategy.Core.Models
{
    public abstract class BaseItem
    {
        private string _name;
        protected BaseItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            _name = name;
        }
        public string GetName() => _name;
        public void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be null or empty.", nameof(newName));
            _name = newName;
        }
    }
}
