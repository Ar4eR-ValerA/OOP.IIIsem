using Shops.Exceptions;

namespace Shops.Models
{
    public class CustomerProductDetails
    {
        public CustomerProductDetails(Product product, int count)
        {
            if (count < 0)
            {
                throw new ShopsException($"Negative count of products: {Count}");
            }

            Count = count;
            Product = product ?? throw new ShopsException("Null argument");
        }

        public Product Product { get; }
        public int Count { get; }
    }
}