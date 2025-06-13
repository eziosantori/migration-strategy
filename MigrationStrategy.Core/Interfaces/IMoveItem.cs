namespace MigrationStrategy.Core.Interfaces
{
    using MigrationStrategy.Core.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the contract for persistence operations on products, categories, and their relationships.
    /// </summary>
    public interface IMoveItem
    {
        string GetName();
    }
}
