using System;
using MigrationStrategy.Core.Interfaces;

namespace MigrationStrategy.Core.Models
{
    public class Category : BaseItem, IMoveItem
    {
        public Category(string name) : base(name) { }
    }
}
