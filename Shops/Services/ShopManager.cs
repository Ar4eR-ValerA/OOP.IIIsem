using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Models;

namespace Shops.Services
{
    public class ShopManager
    {
        private readonly Dictionary<int, Shop> _shops;
        private readonly Dictionary<int, Product> _products;
        private int _currentShopId;
        private int _currentProductId;

        public ShopManager()
        {
            _shops = new Dictionary<int, Shop>();
            _products = new Dictionary<int, Product>();
            _currentShopId = 0;
            _currentProductId = 0;
        }

        public ShopManager(Dictionary<int, Shop> shops, Dictionary<int, Product> products)
        {
            _shops = shops;
            _products = products;
            _currentShopId = 0;
            _currentProductId = 0;
        }

        public IReadOnlyDictionary<int, Shop> Shops => _shops;
        public IReadOnlyDictionary<int, Product> Products => _products;

        public Product RegisterProduct(string name)
        {
            var product = new Product(name, _currentProductId);
            _products.Add(_currentProductId, product);
            _currentProductId++;

            return product;
        }

        public Shop RegisterShop(string name)
        {
            var shop = new Shop(name, _currentShopId);
            _shops.Add(_currentShopId, shop);
            _currentShopId++;

            return shop;
        }

        public Shop FindCheapestShop(CustomerProductDetails customerProductDetails)
        {
            return FindCheapestShop(new List<CustomerProductDetails> { customerProductDetails });
        }

        public Shop FindCheapestShop(List<CustomerProductDetails> customerProductsDetails)
        {
            int minPrice = -1;
            Shop minPriceShop = null;
            IReadOnlyList<Shop> suitableShops = FindShops(customerProductsDetails);

            foreach (Shop shop in suitableShops)
            {
                int sumPrice = customerProductsDetails
                    .Select(customerProduct => shop.FindProduct(customerProduct.Product))
                    .Select(shopProduct => shopProduct.Price)
                    .Sum();

                if (sumPrice >= minPrice && minPrice != -1)
                    continue;

                minPrice = sumPrice;
                minPriceShop = shop;
            }

            return minPriceShop;
        }

        public IReadOnlyList<Shop> FindShops(CustomerProductDetails customerProductDetails)
        {
            return FindShops(new[] { customerProductDetails });
        }

        public IReadOnlyList<Shop> FindShops(IEnumerable<CustomerProductDetails> customerProductsDetails)
        {
            return _shops.Values.Where(shop => shop.IsSuitable(customerProductsDetails)).ToList();
        }
    }
}