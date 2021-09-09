using Shops.Tools;

#nullable enable
namespace Shops.Models
{
    public class ShopProductData
    {
        private int _price;
        private int _count;

        public ShopProductData(Product product, int price, int count)
        {
            if (count < 0)
            {
                throw new ShopsException($"Negative count of products: {_count}");
            }

            if (price <= 0)
            {
                throw new ShopsException($"Negative product's price: {_price}");
            }

            _price = price;
            _count = count;
            Product = product;
        }

        public Product Product { get; }

        public int Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                {
                    throw new ShopsException($"Negative product's price: {_price}");
                }

                _price = value;
            }
        }

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