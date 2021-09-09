#nullable enable
namespace Shops.Models
{
    public class Product
    {
        public Product(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; }
        public int Id { get; }

        public bool Equals(Product value)
        {
            return Id == value.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Product product && Equals(product);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}