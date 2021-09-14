using System.Collections.Generic;
using Shops.Models;
using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        private int _balance;
        private List<CustomerProductDetails> _productsList;

        public Customer(int balance, string name)
        {
            Balance = balance;
            Name = name;
            _productsList = new List<CustomerProductDetails>();
        }

        public int Balance
        {
            get => _balance;
            set
            {
                if (value < 0)
                {
                    throw new ShopsException($"Negative customer's balance");
                }

                _balance = value;
            }
        }

        public string Name { get; }
        public IReadOnlyList<CustomerProductDetails> ProductList => _productsList;
        public void AddProduct(CustomerProductDetails productDetails)
        {
            _productsList.Add(productDetails);
        }
    }
}