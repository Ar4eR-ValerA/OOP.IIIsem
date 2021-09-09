using Shops.Tools;

namespace Shops.Models
{
    public class CustomerProductData
    {
        private int _count;

        public CustomerProductData(int count, Product product)
        {
            if (count < 0)
            {
                throw new ShopsException($"Negative count of products: {_count}");
            }

            _count = count;
            Product = product;
        }

        public Product Product { get; }

        public int Count
        {
            get => _count;
            internal set
            {
                if (value < 0)
                {
                    throw new ShopsException($"Negative count of products: {_count}");
                }

                _count = value;
            }
        }
    }
}