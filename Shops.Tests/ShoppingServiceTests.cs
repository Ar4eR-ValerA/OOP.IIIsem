using System.Collections.Generic;
using System.Linq;
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

            var shopProductDetails = new ShopProductDetails(product, 8, 2);
            shop.AddProduct(shopProductDetails);
            var customerProductDetails = new CustomerProductDetails(3, product);
            shop.Purchase(customer, customerProductDetails);
            
            Assert.AreEqual(1, customer.ProductList.Count);
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

            var shopProductDetails = new ShopProductDetails(product, 8, 2);
            shop.AddProduct(shopProductDetails);
            var customerProductDetails = new CustomerProductDetails(10, product);
            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProductDetails);
            });
        }
        
        [Test]
        public void BuyProductWithNotEnoughMoney_ThrowException()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop = _shopManager.RegisterShop("Apple shop");
            var customer = new Customer(8, "Valera");

            var shopProductDetails = new ShopProductDetails(product, 8, 2);
            shop.AddProduct(shopProductDetails);
            var customerProductDetails = new CustomerProductDetails(5, product);
            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProductDetails);
            });
        }

        [Test]
        public void CustomerWithNegativeBalance_ThrowException()
        {
            Assert.Catch<ShopsException>(() =>
            {
                _ = new Customer(-6, "Valera");
            });
            
            Assert.Catch<ShopsException>(() =>
            {
                var customer = new Customer(0, "Valera");
                customer.Balance = -5;
            });
        }

        [Test]
        public void ShopWithNegativeBalance_ThrowException()
        {
            Assert.Catch<ShopsException>(() =>
            {
                _ = new Shop("Valera's shop", 0, -1);
            });
            
            Assert.Catch<ShopsException>(() =>
            {
                Shop shop = _shopManager.RegisterShop("Valera's shop");
                shop.Balance = -9;
            });
        }
        
        [Test]
        public void BuyProductsList_ShopSellsProductsList()
        {
            Product product1 = _shopManager.RegisterProduct("Apple");
            Product product2 = _shopManager.RegisterProduct("Orange");
            Shop shop = _shopManager.RegisterShop("Apple shop");
            var customer = new Customer(100, "Valera");

            var shopProductDetails1 = new ShopProductDetails(product1, 8, 2);
            var shopProductDetails2 = new ShopProductDetails(product2, 8, 2);
            shop.AddProduct(shopProductDetails1);
            shop.AddProduct(shopProductDetails2);
            
            var customerProduct1 = new CustomerProductDetails(3, product1);
            var customerProduct2 = new CustomerProductDetails(3, product2);
            var customerProducts = new List<CustomerProductDetails> {customerProduct1, customerProduct2};
            
            shop.Purchase(customer, customerProducts);
            
            Assert.AreEqual(2, customer.ProductList.Count);
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

            var shopProductDetails1 = new ShopProductDetails(product1, 3, 2);
            var shopProductDetails2 = new ShopProductDetails(product1, 3, 2);
            shop.AddProduct(shopProductDetails1);
            shop.AddProduct(shopProductDetails2);
            
            var customerProduct1 = new CustomerProductDetails(5, product1);
            var customerProduct2 = new CustomerProductDetails(4, product2);
            var customerProducts = new List<CustomerProductDetails> {customerProduct1, customerProduct2};
            
            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProducts);
            });
        }

        [Test]
        public void FindCheapestShop1_ReturnCheapestShop()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            var shopProductDetails1 = new ShopProductDetails(product, 8, 4);
            var shopProductDetails2 = new ShopProductDetails(product, 8, 2);
            var shopProductDetails3 = new ShopProductDetails(product, 8, 6);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);
            shop3.AddProduct(shopProductDetails3);
            
            var customerProductDetails = new CustomerProductDetails(2, product);
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductDetails);
            
            Assert.AreEqual(shop2, cheapShop);
        }
        
        [Test]
        public void FindCheapestShop2_ReturnCheapestShop()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            var shopProductDetails1 = new ShopProductDetails(product, 8, 4);
            var shopProductDetails2 = new ShopProductDetails(product, 1, 2);
            var shopProductDetails3 = new ShopProductDetails(product, 8, 6);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);
            shop3.AddProduct(shopProductDetails3);
            
            var customerProductDetails = new CustomerProductDetails(2, product);
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductDetails);
            
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

            var shopProductDetails1 = new ShopProductDetails(product1, 8, 4);
            var shopProductDetails2 = new ShopProductDetails(product1, 8, 2);
            var shopProductDetails3 = new ShopProductDetails(product1, 8, 6);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);
            shop3.AddProduct(shopProductDetails3);
            
            var customerProductDetails = new CustomerProductDetails(2, product2);
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductDetails);
            
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

            
            var shopProductDetails1 = new ShopProductDetails(product1, 8, 4);
            var shopProductDetails2 = new ShopProductDetails(product1, 8, 2);
            var shopProductDetails3 = new ShopProductDetails(product1, 8, 6);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);
            shop3.AddProduct(shopProductDetails3);
            
            var customerProductDetails = new CustomerProductDetails(2, product2);
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductDetails);
            
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
            
            var shopProductDetails1 = new ShopProductDetails(product1, 8, 4);
            var shopProductDetails2 = new ShopProductDetails(product2, 8, 2);
            var shopProductDetails3 = new ShopProductDetails(product1, 8, 2);
            var shopProductDetails4 = new ShopProductDetails(product1, 8, 6);
            var shopProductDetails5 = new ShopProductDetails(product2, 8, 1);

            shop1.AddProduct(shopProductDetails1);
            shop1.AddProduct(shopProductDetails2);
            
            shop2.AddProduct(shopProductDetails3);
            
            shop3.AddProduct(shopProductDetails4);
            shop3.AddProduct(shopProductDetails5);
            
            var customerProductsDetails = new List<CustomerProductDetails>
            {
                new CustomerProductDetails(2, product1), 
                new CustomerProductDetails(2, product2) 
            };
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductsDetails);
            
            Assert.AreEqual(shop1, cheapShop);
        }

        [Test] 
        public void FindCheapestShopProductListWithoutProduct_ReturnNull()
        {
            Product product1 = _shopManager.RegisterProduct("Apple");
            Product product2 = _shopManager.RegisterProduct("Orange");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            var shopProductDetails1 = new ShopProductDetails(product1, 8, 4);
            var shopProductDetails2 = new ShopProductDetails(product1, 8, 2);
            var shopProductDetails3 = new ShopProductDetails(product1, 8, 6);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);
            shop3.AddProduct(shopProductDetails3);

            var customerProductsDetails = new List<CustomerProductDetails>
            {
                new CustomerProductDetails(2, product1), 
                new CustomerProductDetails(2, product2) 
            };
            
            Shop cheapShop = _shopManager.FindCheapestShop(customerProductsDetails);
            
            Assert.IsNull(cheapShop);
        }
        
        [Test]
        public void FindShop_ReturnShop()
        {
            Product product = _shopManager.RegisterProduct("Apple");
            Shop shop1 = _shopManager.RegisterShop("Apple1 shop");
            Shop shop2 = _shopManager.RegisterShop("Apple2 shop");
            Shop shop3 = _shopManager.RegisterShop("Apple3 shop");

            var shopProductDetails1 = new ShopProductDetails(product, 8, 4);
            var shopProductDetails2 = new ShopProductDetails(product, 1, 2);
            var shopProductDetails3 = new ShopProductDetails(product, 8, 6);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);
            shop3.AddProduct(shopProductDetails3);
            
            var customerProductDetails = new CustomerProductDetails(2, product);
            
            List<Shop> shops = _shopManager.FindShops(customerProductDetails);
            
            Assert.AreEqual(shop1, shops[0]);
            Assert.AreEqual(shop3, shops[1]);
        }
    }
}