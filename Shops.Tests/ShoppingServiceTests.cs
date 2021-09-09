using System.Collections.Generic;
using NUnit.Framework;
using Shops.Entities;
using Shops.Models;
using Shops.Services;
using Shops.Tools;

namespace Shops.Tests
{
    public class ShoppingServiceTests
    {
        private ShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();

        }

        [Test]
        public void BuyProduct_ShopSellsProduct()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop = _shopManager.RegisterShop("Apple shop");
            var customer = new Customer(10, "Valera");

            shop.AddProduct(product, 8, 2);
            var customerProductData = new CustomerProductData(3, product);
            shop.Purchase(customer, customerProductData);
            
            Assert.AreEqual(5, shop.FindProduct(product).Count);
            Assert.AreEqual(6, shop.Balance);
            Assert.AreEqual(4, customer.Balance);
        }
        
        [Test]
        public void BuyTooManyProducts_TrowException()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop = _shopManager.RegisterShop("Apple shop");
            var customer = new Customer(100, "Valera");

            shop.AddProduct(product, 8, 2);
            var customerProductData = new CustomerProductData(10, product);
            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProductData);
            });
        }
        
        [Test]
        public void BuyProductWithNotEnoughMoney_ThrowException()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop = _shopManager.RegisterShop("Apple shop");
            var customer = new Customer(8, "Valera");

            shop.AddProduct(product, 8, 2);
            var customerProductData = new CustomerProductData(5, product);
            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProductData);
            });
        }

        [Test]
        public void CustomerWithNegativeBalance_ThrowException()
        {
            Assert.Catch<ShopsException>(() =>
            {
                _ = new Customer(-6, "Valera");
            });
        }
        
        [Test]
        public void BuyProductsList_ShopSellsProductsList()
        {
            Product product1 = _shopManager.RegisterProduct("Apple");
            Product product2 = _shopManager.RegisterProduct("Orange");
            Shop shop = _shopManager.RegisterShop("Apple shop");
            var customer = new Customer(100, "Valera");

            shop.AddProduct(product1, 8, 2);
            shop.AddProduct(product2, 8, 2);
            
            var customerProduct1 = new CustomerProductData(3, product1);
            var customerProduct2 = new CustomerProductData(3, product2);
            var customerProducts = new List<CustomerProductData> {customerProduct1, customerProduct2};
            
            shop.Purchase(customer, customerProducts);
            
            Assert.AreEqual(5, shop.FindProduct(product1).Count);
            Assert.AreEqual(5, shop.FindProduct(product2).Count);
            Assert.AreEqual(12, shop.Balance);
            Assert.AreEqual(88, customer.Balance);
        }
        
        [Test]
        public void BuyTooManyProductsList_ThrowException()
        {
            Product product1 = _shopManager.RegisterProduct("Apple");
            Product product2 = _shopManager.RegisterProduct("Orange");
            Shop shop = _shopManager.RegisterShop("Apple shop");
            var customer = new Customer(100, "Valera");

            shop.AddProduct(product1, 3, 2);
            shop.AddProduct(product2, 3, 2);
            
            var customerProduct1 = new CustomerProductData(5, product1);
            var customerProduct2 = new CustomerProductData(4, product2);
            var customerProducts = new List<CustomerProductData> {customerProduct1, customerProduct2};
            
            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProducts);
            });
        }

        [Test]
        public void ChangePrice_PriceChanged()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop = _shopManager.RegisterShop("Apple shop");
            shop.AddProduct(product, 3, 2);

            shop.FindProduct(product).Price = 1;
            
            Assert.AreEqual(1, shop.FindProduct(product).Price);
        }
        
        [Test]
        public void FindCheapestShop1_ReturnCheapestShop()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            shop1.AddProduct(product, 8, 4);
            shop2.AddProduct(product, 8, 2);
            shop3.AddProduct(product, 8, 6);
            
            var customerProductData = new CustomerProductData(2, product);
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductData);
            
            Assert.AreEqual(shop2, cheapShop);
        }
        
        [Test]
        public void FindCheapestShop2_ReturnCheapestShop()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            shop1.AddProduct(product, 8, 4);
            shop2.AddProduct(product, 1, 2);
            shop3.AddProduct(product, 8, 6);
            
            var customerProductData = new CustomerProductData(2, product);
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductData);
            
            Assert.AreEqual(shop1, cheapShop);
        }
        
        [Test]
        public void FindCheapestShopWithoutThisProduct_ReturnNull()
        {
            Product product1 = _shopManager.RegisterProduct("Apple");
            Product product2 = _shopManager.RegisterProduct("Orange");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            shop1.AddProduct(product1, 8, 4);
            shop2.AddProduct(product1, 8, 2);
            shop3.AddProduct(product1, 8, 6);
            
            var customerProductData = new CustomerProductData(2, product2);
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductData);
            
            Assert.IsNull(cheapShop);
        }
        
        [Test]
        public void FindCheapestShopWithoutMoney_ReturnNull()
        {
            Product product1 = _shopManager.RegisterProduct("Apple");
            Product product2 = _shopManager.RegisterProduct("Orange");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            shop1.AddProduct(product1, 8, 4);
            shop2.AddProduct(product1, 8, 2);
            shop3.AddProduct(product1, 8, 6);
            
            var customerProductData = new CustomerProductData(2, product2);
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductData);
            
            Assert.IsNull(cheapShop);
        }
        
        [Test]
        public void FindCheapestShopProductList_ReturnCheapestShop()
        {
            Product product1 = _shopManager.RegisterProduct("Apple");
            Product product2 = _shopManager.RegisterProduct("Orange");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            shop1.AddProduct(product1, 8, 4);
            shop1.AddProduct(product2, 8, 2);
            
            shop2.AddProduct(product1, 8, 2);
            
            shop3.AddProduct(product1, 8, 6);
            shop3.AddProduct(product2, 8, 1);
            
            var customerProductsData = new List<CustomerProductData>
            {
                new CustomerProductData(2, product1), 
                new CustomerProductData(2, product2) 
            };
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductsData);
            
            Assert.AreEqual(shop1, cheapShop);
        }

        [Test] public void FindCheapestShopProductListWithoutProduct_ReturnNull()
        {
            Product product1 = _shopManager.RegisterProduct("Apple");
            Product product2 = _shopManager.RegisterProduct("Orange");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            shop1.AddProduct(product1, 8, 4);
            shop2.AddProduct(product1, 8, 2);
            shop3.AddProduct(product1, 8, 6);

            var customerProductsData = new List<CustomerProductData>
            {
                new CustomerProductData(2, product1), 
                new CustomerProductData(2, product2) 
            };
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductsData);
            
            Assert.IsNull(cheapShop);
        }
    }
}