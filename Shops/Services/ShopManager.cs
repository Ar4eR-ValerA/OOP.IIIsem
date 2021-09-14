using System.Collections.Generic;
using Shops.Entities;
using Shops.Models;

namespace Shops.Services
{
    public class ShopManager
    {
        private List<Shop> _shops = new List<Shop>();
        private List<Product> _products = new List<Product>();

        public IReadOnlyList<Shop> Shops => _shops;
        public IReadOnlyList<Product> Products => _products;

        public Product RegisterProduct(string name)
        {
            var product = new Product(name, _products.Count);
            _products.Add(product);

            return product;
        }

        public Shop RegisterShop(string name)
        {
            var shop = new Shop(name, _shops.Count, 0);
            _shops.Add(shop);

            return shop;
        }

        public Shop FindCheapestShop(CustomerProductDetails customerProductDetails)
        {
            int minPrice = -1;
            Shop minPriceShop = null;

            foreach (Shop shop in _shops)
            {
                ShopProductDetails currentProduct = shop.FindProduct(customerProductDetails.Product);
                if (currentProduct == null)
                {
                    continue;
                }

                if ((currentProduct.Price < minPrice || minPrice == -1) &&
                    currentProduct.Count >= customerProductDetails.Count)
                {
                    minPrice = currentProduct.Price;
                    minPriceShop = shop;
                }
            }

            return minPriceShop;
        }

        public Shop FindCheapestShop(List<CustomerProductDetails> customerProductsDetails)
        {
            int minPrice = -1;
            Shop minPriceShop = null;

            foreach (Shop shop in _shops)
            {
                int sumPrice = 0;

                foreach (CustomerProductDetails customerProduct in customerProductsDetails)
                {
                    ShopProductDetails currentProduct = shop.FindProduct(customerProduct.Product);
                    if (currentProduct != null)
                    {
                        sumPrice += currentProduct.Price;
                    }
                    else
                    {
                        sumPrice = -1;
                        break;
                    }
                }

                if ((sumPrice < minPrice || minPrice == -1) && sumPrice != -1)
                {
                    minPrice = sumPrice;
                    minPriceShop = shop;
                }
            }

            return minPriceShop;
        }

        public List<Shop> FindShops(CustomerProductDetails customerProductDetails)
        {
            var shops = new List<Shop>();

            foreach (Shop shop in _shops)
            {
                ShopProductDetails currentProduct = shop.FindProduct(customerProductDetails.Product);
                if (currentProduct == null)
                {
                    continue;
                }

                if (currentProduct.Count >= customerProductDetails.Count)
                {
                    shops.Add(shop);
                }
            }

            return shops;
        }

        public List<Shop> FindShops(List<CustomerProductDetails> customerProductsDetails)
        {
            var shops = new List<Shop>();

            foreach (Shop shop in _shops)
            {
                bool suitableFl = true;

                foreach (CustomerProductDetails customerProduct in customerProductsDetails)
                {
                    ShopProductDetails currentProduct = shop.FindProduct(customerProduct.Product);
                    if (currentProduct == null)
                    {
                        suitableFl = false;
                        break;
                    }
                }

                if (suitableFl)
                {
                    shops.Add(shop);
                }
            }

            return shops;
        }
    }
}