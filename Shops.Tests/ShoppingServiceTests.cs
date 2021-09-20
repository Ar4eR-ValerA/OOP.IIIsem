using System.Collections.Generic;
using NUnit.Framework;
using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;
using Shops.Services;

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
        [TestCase(10, 8, 2, 3)]
        public void BuyProduct_ShopSellsProduct(
            int customerBalanceBefore,
            int shopProductCountBefore,
            int shopProductPrice,
            int customerProductCount)
        {
            Product product = _shopManager.RegisterProduct(new Product("Apple"));
            Shop shop = _shopManager.RegisterShop(new Shop("Apple shop"));
            var customer = new Customer(customerBalanceBefore, "Valera");

            var shopProductDetails = new ShopProductDetails(product, shopProductCountBefore, shopProductPrice);
            shop.AddProduct(shopProductDetails);
            var customerProductDetails = new CustomerProductDetails(product, customerProductCount);
            shop.Purchase(customer, customerProductDetails);

            Assert.AreEqual(shopProductCountBefore - customerProductCount, shop.FindProduct(product).Count);
            Assert.AreEqual(customerProductCount * shopProductPrice, shop.Balance);
            Assert.AreEqual(customerBalanceBefore - shop.Balance, customer.Balance);
        }

        [Test]
        [TestCase(100, 2, 2, 10)]
        public void BuyTooManyProducts_TrowException(
            int customerBalanceBefore,
            int shopProductCountBefore,
            int shopProductPrice,
            int customerProductCount)
        {
            Product product = _shopManager.RegisterProduct(new Product("Apple"));
            Shop shop = _shopManager.RegisterShop(new Shop("Apple shop"));
            var customer = new Customer(customerBalanceBefore, "Valera");

            var shopProductDetails = new ShopProductDetails(product, shopProductCountBefore, shopProductPrice);
            shop.AddProduct(shopProductDetails);
            var customerProductDetails = new CustomerProductDetails(product, customerProductCount);
            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProductDetails);
            });
        }

        [Test]
        [TestCase(8, 8, 2, 5)]
        public void BuyProductWithNotEnoughMoney_ThrowException(
            int customerBalanceBefore,
            int shopProductCountBefore,
            int shopProductPrice,
            int customerProductCount)
        {
            Product product = _shopManager.RegisterProduct(new Product("Apple"));
            Shop shop = _shopManager.RegisterShop(new Shop("Apple shop"));
            var customer = new Customer(customerBalanceBefore, "Valera");

            var shopProductDetails = new ShopProductDetails(product, shopProductCountBefore, shopProductPrice);
            shop.AddProduct(shopProductDetails);
            var customerProductDetails = new CustomerProductDetails(product, customerProductCount);
            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProductDetails);
            });
        }

        [Test]
        [TestCase(-6)]
        public void CustomerWithNegativeBalance_ThrowException(int negativeBalance)
        {
            Assert.Catch<ShopsException>(() =>
            {
                _ = new Customer(negativeBalance, "Valera");
            });
        }

        [Test]
        [TestCase(-6, 0)]
        public void ShopWithNegativeBalance_ThrowException(int negativeBalance, int shopId)
        {
            Assert.Catch<ShopsException>(() =>
            {
                _ = new Shop("Valera's shop", shopId, negativeBalance);
            });
        }

        [Test]
        [TestCase(100, 8, 2, 8, 2, 3, 3)]
        public void BuyProductsList_ShopSellsProductsList(
            int customerBalanceBefore,
            int shopProduct1CountBefore,
            int shopProduct1Price,
            int shopProduct2CountBefore,
            int shopProduct2Price,
            int customerProduct1Count,
            int customerProduct2Count)
        {
            Product product1 = _shopManager.RegisterProduct(new Product("Apple"));
            Product product2 = _shopManager.RegisterProduct(new Product("Orange"));
            Shop shop = _shopManager.RegisterShop(new Shop("Apple shop"));
            var customer = new Customer(customerBalanceBefore, "Valera");

            var shopProductDetails1 = new ShopProductDetails(product1, shopProduct1CountBefore, shopProduct1Price);
            var shopProductDetails2 = new ShopProductDetails(product2, shopProduct2CountBefore, shopProduct2Price);
            shop.AddProduct(shopProductDetails1);
            shop.AddProduct(shopProductDetails2);

            var customerProduct1 = new CustomerProductDetails(product1, customerProduct1Count);
            var customerProduct2 = new CustomerProductDetails(product2, customerProduct2Count);
            var customerProducts = new List<CustomerProductDetails> { customerProduct1, customerProduct2 };

            shop.Purchase(customer, customerProducts);

            Assert.AreEqual(shopProduct1CountBefore - customerProduct1Count, shop.FindProduct(product1).Count);
            Assert.AreEqual(shopProduct2CountBefore - customerProduct2Count, shop.FindProduct(product2).Count);
            Assert.AreEqual(
                shopProduct1Price * customerProduct1Count + shopProduct2Price * customerProduct2Count,
                shop.Balance);
            Assert.AreEqual(customerBalanceBefore - shop.Balance, customer.Balance);
        }

        [Test]
        [TestCase(100, 3, 2, 3, 2, 5, 4)]
        public void BuyTooManyProductsList_ThrowException(
            int customerBalanceBefore,
            int shopProduct1CountBefore,
            int shopProduct1Price,
            int shopProduct2CountBefore,
            int shopProduct2Price,
            int customerProduct1Count,
            int customerProduct2Count)
        {
            Product product1 = _shopManager.RegisterProduct(new Product("Apple"));
            Product product2 = _shopManager.RegisterProduct(new Product("Orange"));
            Shop shop = _shopManager.RegisterShop(new Shop("Apple shop"));
            var customer = new Customer(customerBalanceBefore, "Valera");

            var shopProductDetails1 = new ShopProductDetails(product1, shopProduct1CountBefore, shopProduct1Price);
            var shopProductDetails2 = new ShopProductDetails(product1, shopProduct2CountBefore, shopProduct2Price);
            shop.AddProduct(shopProductDetails1);
            shop.AddProduct(shopProductDetails2);

            var customerProduct1 = new CustomerProductDetails(product1, customerProduct1Count);
            var customerProduct2 = new CustomerProductDetails(product2, customerProduct2Count);
            var customerProducts = new List<CustomerProductDetails> { customerProduct1, customerProduct2 };

            Assert.Catch<ShopsException>(() =>
            {
                shop.Purchase(customer, customerProducts);
            });
        }

        [Test]
        [TestCase(8, 2, 8, 4, 4)]
        [TestCase(8, 4, 2, 2, 4)]
        public void FindCheapestShop_ReturnCheapestShop(
            int chosenShopProductCountBefore,
            int chosenShopProductPrice,
            int shopProductCountBefore,
            int shopProductPrice,
            int customerProductCount)
        {
            Product product = _shopManager.RegisterProduct(new Product("Apple"));
            Shop shop1 = _shopManager.RegisterShop(new Shop("Apple1 shop"));
            Shop shop2 = _shopManager.RegisterShop(new Shop("Apple2 shop"));

            var shopProductDetails1 =
                new ShopProductDetails(product, chosenShopProductCountBefore, chosenShopProductPrice);
            var shopProductDetails2 = new ShopProductDetails(product, shopProductCountBefore, shopProductPrice);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);

            var customerProductDetails = new CustomerProductDetails(product, customerProductCount);

            Shop cheapShop = _shopManager.FindCheapestShop(customerProductDetails);

            Assert.AreEqual(shop1, cheapShop);
        }

        [Test]
        [TestCase(8, 4, 8, 2, 2)]
        public void FindCheapestShopWithoutThisProduct_ReturnNull(
            int shop1ProductCountBefore,
            int shop1ProductPrice,
            int shop2ProductCountBefore,
            int shop2ProductPrice,
            int customerProductCount)
        {
            Product product1 = _shopManager.RegisterProduct(new Product("Apple"));
            Product product2 = _shopManager.RegisterProduct(new Product("Orange"));
            Shop shop1 = _shopManager.RegisterShop(new Shop("Apple1 shop"));
            Shop shop2 = _shopManager.RegisterShop(new Shop("Apple2 shop"));

            var shopProductDetails1 = new ShopProductDetails(product1, shop1ProductCountBefore, shop1ProductPrice);
            var shopProductDetails2 = new ShopProductDetails(product1, shop2ProductCountBefore, shop2ProductPrice);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);

            var customerProductDetails = new CustomerProductDetails(product2, customerProductCount);

            Shop cheapShop = _shopManager.FindCheapestShop(customerProductDetails);

            Assert.IsNull(cheapShop);
        }

        [Test]
        [TestCase(8, 4, 8, 2, 2)]
        public void FindCheapestShopWithoutMoney_ReturnNull(
            int shop1ProductCountBefore,
            int shop1ProductPrice,
            int shop2ProductCountBefore,
            int shop2ProductPrice,
            int customerProductCount)
        {
            Product product1 = _shopManager.RegisterProduct(new Product("Apple"));
            Product product2 = _shopManager.RegisterProduct(new Product("Orange"));
            Shop shop1 = _shopManager.RegisterShop(new Shop("Apple1 shop"));
            Shop shop2 = _shopManager.RegisterShop(new Shop("Apple2 shop"));

            var shopProductDetails1 = new ShopProductDetails(product1, shop1ProductCountBefore, shop1ProductPrice);
            var shopProductDetails2 = new ShopProductDetails(product1, shop2ProductCountBefore, shop2ProductPrice);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);

            var customerProductDetails = new CustomerProductDetails(product2, customerProductCount);

            Shop cheapShop = _shopManager.FindCheapestShop(customerProductDetails);

            Assert.IsNull(cheapShop);
        }

        [Test]
        [TestCase(8, 4, 8, 2, 2)]
        public void FindCheapestShopProductList_ReturnCheapestShop(
            int shop1ProductCountBefore,
            int shop1ProductPrice,
            int shop2ProductCountBefore,
            int shop2ProductPrice,
            int customerProductCount)
        {
            Product product1 = _shopManager.RegisterProduct(new Product("Apple"));
            Product product2 = _shopManager.RegisterProduct(new Product("Orange"));
            Shop shop1 = _shopManager.RegisterShop(new Shop("Apple1 shop"));
            Shop shop2 = _shopManager.RegisterShop(new Shop("Apple2 shop"));

            var shopProductDetails1 = new ShopProductDetails(product1, shop1ProductCountBefore, shop1ProductPrice);
            var shopProductDetails2 = new ShopProductDetails(product2, shop2ProductCountBefore, shop2ProductPrice);

            shop1.AddProduct(shopProductDetails1);
            shop1.AddProduct(shopProductDetails2);

            shop2.AddProduct(shopProductDetails2);

            var customerProductsDetails = new List<CustomerProductDetails>
            {
                new CustomerProductDetails(product1, customerProductCount),
                new CustomerProductDetails(product2, customerProductCount)
            };

            Shop cheapShop = _shopManager.FindCheapestShop(customerProductsDetails);

            Assert.AreEqual(shop1, cheapShop);
        }

        [Test]
        [TestCase(8, 4, 2)]
        public void FindCheapestShopProductListWithoutProduct_ReturnNull(
            int shopProductCountBefore,
            int shopProductPrice,
            int customerProductCount)
        {
            Product product1 = _shopManager.RegisterProduct(new Product("Apple"));
            Product product2 = _shopManager.RegisterProduct(new Product("Orange"));
            Shop shop1 = _shopManager.RegisterShop(new Shop("Apple1 shop"));
            Shop shop2 = _shopManager.RegisterShop(new Shop("Apple2 shop"));

            var shopProductDetails = new ShopProductDetails(product1, shopProductCountBefore, shopProductPrice);
            shop1.AddProduct(shopProductDetails);
            shop2.AddProduct(shopProductDetails);

            var customerProductsDetails = new List<CustomerProductDetails>
            {
                new CustomerProductDetails(product1, customerProductCount),
                new CustomerProductDetails(product2, customerProductCount)
            };

            Shop cheapShop = _shopManager.FindCheapestShop(customerProductsDetails);

            Assert.IsNull(cheapShop);
        }

        [Test]
        [TestCase(8, 4, 1, 2, 0)]
        public void FindShop_ReturnShop(
            int shop1ProductCountBefore,
            int shop1ProductPrice,
            int shop2ProductCountBefore,
            int shop2ProductPrice,
            int customerProductCount)
        {
            Product product = _shopManager.RegisterProduct(new Product("Apple"));
            Shop shop1 = _shopManager.RegisterShop(new Shop("Apple1 shop"));
            Shop shop2 = _shopManager.RegisterShop(new Shop("Apple2 shop"));

            var shopProductDetails1 = new ShopProductDetails(product, shop1ProductCountBefore, shop1ProductPrice);
            var shopProductDetails2 = new ShopProductDetails(product, shop2ProductCountBefore, shop2ProductPrice);
            shop1.AddProduct(shopProductDetails1);
            shop2.AddProduct(shopProductDetails2);

            var customerProductDetails = new CustomerProductDetails(product, customerProductCount);

            IReadOnlyList<Shop> shops = _shopManager.FindShops(customerProductDetails);

            Assert.AreEqual(shop1, shops[0]);
            Assert.AreEqual(shop2, shops[1]);
        }
    }
}