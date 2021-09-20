using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Models;
using Shops.Services;

namespace Shops.Ui.Tools
{
    public class Inputter
    {
        private readonly Asker _asker;

        public Inputter()
        {
            _asker = new Asker();
        }

        public int InputBalance()
        {
            return _asker.AskInt("Enter your balance:\n");
        }

        public string InputName()
        {
            return _asker.AskString("Enter the name:\n");
        }

        public int InputShopId(IEnumerable<int> shops)
        {
            return _asker.AskChoices(
                "Enter id of shop:",
                shops);
        }

        public int InputProductId(IEnumerable<int> products)
        {
            return _asker.AskChoices(
                "Enter id of product:",
                products);
        }

        public int InputProductCount()
        {
            return _asker.AskInt("Enter amount of this product:\n");
        }

        public int InputProductPrice()
        {
            return _asker.AskInt("Enter price of this product:\n");
        }

        public List<CustomerProductDetails> InputShoppingList(IReadOnlyDictionary<int, Product> products)
        {
            var customerProducts = new List<CustomerProductDetails>();

            string next = "next";
            while (next != "stop")
            {
                int productId = InputProductId(products.Keys);
                int productCount = InputProductCount();

                customerProducts.Add(new CustomerProductDetails(products[productId], productCount));

                next = _asker.AskChoices(
                    "Another product or enough?",
                    new[] { "next", "stop" });
            }

            return customerProducts;
        }

        public Shop InputShopForPurchase(List<CustomerProductDetails> customerShoppingList, ShopManager shopManager)
        {
            string shopChoice = _asker.AskChoices(
                "Special shop or the cheapest?",
                new[] { "cheapest", "special" });

            if (shopChoice == "cheapest")
            {
                return shopManager.FindCheapestShop(customerShoppingList);
            }

            IReadOnlyList<Shop> shops = shopManager.FindShops(customerShoppingList);
            int shopId = _asker.AskChoices("Enter id of shop", shops.Select(shop => shop.Id).ToList());

            return shopManager.Shops[shopId];
        }
    }
}