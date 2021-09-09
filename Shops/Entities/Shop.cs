using System.Collections.Generic;
using Shops.Models;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private List<ShopProductData> _productsDataList;
        private int _balance;

        public Shop(string name, int id, int balance)
        {
            if (balance < 0)
            {
                throw new ShopsException($"Negative shop's balance: {_balance}");
            }

            _balance = balance;

            Name = name;
            Id = id;
            Balance = balance;
            _productsDataList = new List<ShopProductData>();
        }

        public IReadOnlyList<ShopProductData> ProductsDataList => _productsDataList;
        public int Id { get; }
        public string Name { get; }

        public int Balance
        {
            get => _balance;
            internal set
            {
                if (value < 0)
                {
                    throw new ShopsException($"Negative shop's balance: {_balance}");
                }

                _balance = value;
            }
        }

        public ShopProductData AddProduct(Product product, int count, int price)
        {
            var productData = new ShopProductData(product, price, count);

            ShopProductData currentProductData = FindProduct(productData.Product);
            if (currentProductData != null)
            {
                currentProductData.Count += productData.Count;
                currentProductData.Price = productData.Price;
            }
            else
            {
                _productsDataList.Add(
                    new ShopProductData(productData.Product, productData.Price, productData.Count));
            }

            return productData;
        }

        public void Purchase(Customer customer, CustomerProductData shoppingList)
        {
            ShopProductData requiredProduct = FindProduct(shoppingList.Product);

            if (requiredProduct == null)
            {
                throw new ShopsException($"Shop hasn't this product: {shoppingList.Product.Name}");
            }

            if (requiredProduct.Count < shoppingList.Count)
            {
                throw new ShopsException($"Shop hasn't enough this product: {shoppingList.Product.Name}\n" +
                                         $"Shop has: {requiredProduct.Count}, Customer required: {shoppingList.Count}");
            }

            EnoughProductsCase(customer, requiredProduct, shoppingList);
        }

        public void Purchase(Customer customer, List<CustomerProductData> shoppingList)
        {
            foreach (CustomerProductData product in shoppingList)
            {
                Purchase(customer, product);
            }
        }

        public ShopProductData FindProduct(Product product)
        {
            foreach (ShopProductData productData in ProductsDataList)
            {
                if (productData.Product.Equals(product))
                {
                    return productData;
                }
            }

            return null;
        }

        private void MakeTransaction(Customer customer, int money)
        {
            customer.Balance -= money;
            Balance += money;
        }

        private void EnoughProductsCase(
            Customer customer,
            ShopProductData shopProduct,
            CustomerProductData shoppingList)
        {
            int price = shopProduct.Price * shoppingList.Count;

            if (price > customer.Balance)
            {
                throw new ShopsException($"Customer hasn't enough money: {customer.Balance}, " +
                                         $"Price is: {price}");
            }

            MakeTransaction(customer, price);
            ChangeShopProductsList(shopProduct, shoppingList.Count);
        }

        private void ChangeShopProductsList(ShopProductData product, int amount)
        {
            product.Count -= amount;
            if (product.Count == 0)
            {
                _productsDataList.Remove(product);
            }
        }
    }
}