using Shops.Tools;

namespace Shops.Models
{
    public class CustomerProductDetails
    {
        public CustomerProductDetails(int count, Product product)
        {
            if (count < 0)
            {
                throw new ShopsException($"Negative count of products: {Count}");
            }

            Count = count;
            Product = product;
        }

        public Product Product { get; }

        public int Count { get; }
    }
}