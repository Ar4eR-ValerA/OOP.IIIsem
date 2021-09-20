using System.Collections.Generic;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities
{
    public class Customer
    {
        private readonly List<CustomerProductDetails> _productsList;
        private int _balance;

        public Customer(int balance, string name)
        {
            Balance = balance;
            Name = name ?? throw new ShopsException("Null argument");
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