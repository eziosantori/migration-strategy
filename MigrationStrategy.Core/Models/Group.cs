namespace MigrationStrategy.Core.Models
{
    public class Group
    {
        private readonly string _name;
        public Group(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Group name cannot be null or empty.", nameof(name));
            _name = name;
        }
        public string GetName() => _name;
    }
}
