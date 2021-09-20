using System;
using Shops.Exceptions;

#nullable enable
namespace Shops.Models
{
    public class Product
    {
        private static int _idCounter = 1;
        public Product(string name)
        {
            Name = name ?? throw new ShopsException("Null argument");
            Id = _idCounter++;
        }

        public string Name { get; }
        public int Id { get; }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Product? value)
        {
            return Id == value?.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Product product && Equals(product);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}