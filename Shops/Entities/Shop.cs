using System.Collections.Generic;
using System.Linq;
using Shops.Models;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private List<ShopProductDetails> _productsDetailsList;
        private int _balance;

        public Shop(string name, int id, int balance)
        {
            Balance = balance;
            Name = name;
            Id = id;
            Balance = balance;
            _productsDetailsList = new List<ShopProductDetails>();
        }

        public IReadOnlyList<ShopProductDetails> ProductsDetailsList => _productsDetailsList;
        public int Id { get; }
        public string Name { get; }

        public int Balance
        {
            get => _balance;
            set
            {
                if (value < 0)
                {
                    throw new ShopsException($"Negative shop's balance: {_balance}");
                }

                _balance = value;
            }
        }

        public void AddProduct(ShopProductDetails productDetails)
        {
            _productsDetailsList.Add(productDetails);
        }

        public void Purchase(Customer customer, CustomerProductDetails shoppingList)
        {
            ShopProductDetails requiredProduct = FindProduct(shoppingList.Product);

            if (requiredProduct == null)
            {
                throw new ShopsException($"Shop hasn't this product: {shoppingList.Product.Name}");
            }

            if (requiredProduct.Count < shoppingList.Count)
            {
                throw new ShopsException($"Shop hasn't enough this product: {shoppingList.Product.Name}\n" +
                                         $"Shop has: {requiredProduct.Count}, Customer required: {shoppingList.Count}");
            }

            int totalPrice = requiredProduct.Price * shoppingList.Count;

            if (totalPrice > customer.Balance)
            {
                throw new ShopsException($"Customer hasn't enough money: {customer.Balance}, " +
                                         $"Price is: {totalPrice}");
            }

            MakeTransaction(customer, totalPrice);
            ChangeShopProductsList(requiredProduct, shoppingList.Count);
            ChangeCustomerProductsList(customer, requiredProduct, shoppingList.Count);
        }

        public void Purchase(Customer customer, List<CustomerProductDetails> shoppingList)
        {
            foreach (CustomerProductDetails product in shoppingList)
            {
                Purchase(customer, product);
            }
        }

        public ShopProductDetails FindProduct(Product product)
        {
            return ProductsDetailsList.FirstOrDefault(productDetails => productDetails.Product.Equals(product));
        }

        private void MakeTransaction(Customer customer, int money)
        {
            customer.Balance -= money;
            Balance += money;
        }

        private void ChangeShopProductsList(ShopProductDetails product, int count)
        {
            int totalCount = product.Count - count;
            _productsDetailsList.Remove(product);

            _productsDetailsList.Add(new ShopProductDetails(product.Product, totalCount, product.Price));
        }

        private void ChangeCustomerProductsList(Customer customer, ShopProductDetails shopProduct, int count)
        {
            customer.AddProduct(new CustomerProductDetails(count, shopProduct.Product));
        }
    }
}