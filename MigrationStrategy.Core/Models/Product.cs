using System;
using MigrationStrategy.Core.Interfaces;

namespace MigrationStrategy.Core.Models
{
    public class Product : BaseItem, IMoveItem
    {
        public Product(string name) : base(name) { }
    }
}
