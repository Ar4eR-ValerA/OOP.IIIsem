using Shops.Exceptions;

#nullable enable
namespace Shops.Models
{
    public class ShopProductDetails
    {
        public ShopProductDetails(Product product, int count, int price)
        {
            if (price <= 0)
            {
                throw new ShopsException($"Negative product's price: {Price}");
            }

            if (count < 0)
            {
                throw new ShopsException($"Negative count of products: {Count}");
            }

            Price = price;
            Count = count;
            Product = product ?? throw new ShopsException("Null argument");
        }

        public Product Product { get; }
        public int Price { get; }
        public int Count { get; }
    }
}