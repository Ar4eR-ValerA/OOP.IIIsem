using Shops.Exceptions;

#nullable enable
namespace Shops.Models
{
    public class Product
    {
        public Product(string name, int id)
        {
            Name = name ?? throw new ShopsException("Null argument");
            Id = id;
        }

        public string Name { get; }
        public int Id { get; }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Product? value)
        {
            return value != null && Id == value.Id;
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