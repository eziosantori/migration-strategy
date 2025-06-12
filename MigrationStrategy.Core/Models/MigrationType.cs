namespace MigrationStrategy.Core.Models
{
    public enum MigrationType
    {
        COPY_SINGLE,
        MOVE_SINGLE,
        COPY_MANY,
        MOVE_MANY
    }

    public static class MigrationTypeExtensions
    {
        public static string ToStringFast(this MigrationType type)
        {
            return type switch
            {
                MigrationType.COPY_SINGLE => nameof(MigrationType.COPY_SINGLE),
                MigrationType.MOVE_SINGLE => nameof(MigrationType.MOVE_SINGLE),
                MigrationType.COPY_MANY => nameof(MigrationType.COPY_MANY),
                MigrationType.MOVE_MANY => nameof(MigrationType.MOVE_MANY),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static MigrationType Parse(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException("Value cannot be null or empty.", nameof(str));
            return str switch
            {
                nameof(MigrationType.COPY_SINGLE) => MigrationType.COPY_SINGLE,
                nameof(MigrationType.MOVE_SINGLE) => MigrationType.MOVE_SINGLE,
                nameof(MigrationType.COPY_MANY) => MigrationType.COPY_MANY,
                nameof(MigrationType.MOVE_MANY) => MigrationType.MOVE_MANY,
                _ => throw new ArgumentException($"Invalid MigrationType: {str}", nameof(str))
            };
        }
    }
}
