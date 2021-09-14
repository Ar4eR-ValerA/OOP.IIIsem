using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Models;
using Shops.Services;
using Spectre.Console;

namespace Shops.Console_UI
{
    public class Actions
    {
        private Asking _asking;

        public Actions()
        {
            _asking = new Asking();
        }

        public void RenderMainTable(ShopManager shopManager)
        {
            var mainTable = new Table();
            var shopsTable = new Table();
            var productsTable = new Table();

            CreateMainTable(mainTable, shopsTable, productsTable);

            foreach (Shop shop in shopManager.Shops)
            {
                AddRow(shopsTable, shop.Name, shop.Id);
            }

            foreach (Product product in shopManager.Products)
            {
                AddRow(productsTable, product.Name, product.Id);
            }

            AnsiConsole.Render(mainTable);
        }

        public void RegisterShop(ShopManager shopManager)
        {
            string title = _asking.AskString("Enter the title:\n");
            shopManager.RegisterShop(title);
        }

        public void RegisterProduct(ShopManager shopManager)
        {
            string title = _asking.AskString("Enter the title:\n");
            shopManager.RegisterProduct(title);
        }

        public void AddProducts(ShopManager shopManager)
        {
            if (shopManager.Shops.Count == 0)
            {
                _asking.AskChoices("There are no shops", new[] { "exit" });
                return;
            }

            if (shopManager.Products.Count == 0)
            {
                _asking.AskChoices("There are no products", new[] { "exit" });
                return;
            }

            int shopId = _asking.AskChoices(
                "Enter id of shop:",
                shopManager.Shops.Select(shop => shop.Id).ToList());

            int productId = _asking.AskChoices(
                "Enter id of product:",
                shopManager.Products.Select(product => product.Id).ToList());

            int productCount = _asking.AskInt("Enter amount of this product:\n");
            int productPrice = _asking.AskInt("Enter price of this product:\n");

            var shopProduct = new ShopProductDetails(shopManager.Products[productId], productCount, productPrice);
            shopManager.Shops[shopId].AddProduct(shopProduct);
        }

        public void MakePurchase(Customer customer, ShopManager shopManager)
        {
            List<CustomerProductDetails> customerShoppingList = GetCustomerProducts(shopManager);
            if (customerShoppingList.Count == 0)
            {
                return;
            }

            string shopChoice = _asking.AskChoices(
                "Special shop or the cheapest?",
                new[] { "cheapest", "special" });

            Shop chosenShop;
            if (shopChoice == "cheapest")
            {
                chosenShop = shopManager.FindCheapestShop(customerShoppingList);
                if (chosenShop == null)
                {
                    _asking.AskChoices("There is no suitable shop", new[] { "exit" });
                    return;
                }
            }
            else
            {
                List<Shop> shops = shopManager.FindShops(customerShoppingList);
                if (shops.Count == 0)
                {
                    _asking.AskChoices("There are no suitable shops", new[] { "exit" });
                    return;
                }

                int shopId = _asking.AskChoices("Enter id of shop", shops.Select(shop => shop.Id).ToList());
                chosenShop = shopManager.Shops[shopId];
            }

            chosenShop.Purchase(customer, customerShoppingList);
        }

        public void ShowShopProducts(ShopManager shopManager)
        {
            AnsiConsole.Clear();

            if (shopManager.Shops.Count == 0)
            {
                _asking.AskChoices("There are no shops", new[] { "exit" });
                return;
            }

            int shopId = _asking.AskChoices("Enter id of shop", shopManager.Shops.Select(shop => shop.Id).ToList());

            var shopTable = new Table
            {
                Title = new TableTitle($"{shopManager.Shops[shopId].Name} id: {shopManager.Shops[shopId].Id}"),
            };

            shopTable.AddColumns("Product name", "ID", "Price", "Count");
            foreach (ShopProductDetails product in shopManager.Shops[shopId].ProductsDetailsList)
            {
                AddRow(shopTable, product.Product.ToString(), product.Product.Id, product.Price, product.Count);
            }

            AnsiConsole.Render(shopTable);

            _asking.AskChoices(string.Empty, new[] { "exit" });
        }

        public Customer CreateCustomer()
        {
            string customerName = _asking.AskString("Enter your name:\n");

            int customerBalance = _asking.AskInt("Enter your balance:\n");
            return new Customer(customerBalance, customerName);
        }

        public void ShowCustomerDetails(Customer customer)
        {
            AnsiConsole.Write($"{customer.Name}: {customer.Balance}$\n");
        }

        public void ChangeCustomerBalance(Customer customer)
        {
            int customerBalance = _asking.AskInt("Enter your new balance:\n");
            customer.Balance = customerBalance;
        }

        public void ShowCustomerProducts(Customer customer)
        {
            AnsiConsole.Clear();

            var customerTable = new Table
            {
                Title = new TableTitle($"{customer.Name}: {customer.Balance}\n"),
            };
            customerTable.AddColumns("Product name", "ID", "Count");
            foreach (CustomerProductDetails product in customer.ProductList)
            {
                AddRow(customerTable, product.Product.ToString(), product.Product.Id, product.Count);
            }

            AnsiConsole.Render(customerTable);

            _asking.AskChoices(string.Empty, new[] { "exit" });
        }

        private void CreateMainTable(Table mainTable, Table shopsTable, Table productsTable)
        {
            mainTable.AddColumn("Shops");
            mainTable.AddColumn("Products");
            mainTable.AddRow(shopsTable, productsTable);

            shopsTable.AddColumn("Name:");
            shopsTable.AddColumn("id:");
            shopsTable.Border = TableBorder.None;

            productsTable.AddColumn("Name:");
            productsTable.AddColumn("id:");
            productsTable.Border = TableBorder.None;
        }

        private List<CustomerProductDetails> GetCustomerProducts(ShopManager shopManager)
        {
            var customerProducts = new List<CustomerProductDetails>();

            string next = "next";
            while (next != "stop")
            {
                if (shopManager.Products.Count == 0)
                {
                    _asking.AskChoices("There are no products", new[] { "exit" });
                    return customerProducts;
                }

                int productId = _asking.AskChoices(
                    "Enter id of product:",
                    shopManager.Products.Select(product => product.Id).ToList());
                int productCount = _asking.AskInt("Enter amount of this product\n");

                customerProducts.Add(new CustomerProductDetails(productCount, shopManager.Products[productId]));

                next = _asking.AskChoices(
                    "Another product or enough",
                    new[] { "next", "stop" });
            }

            return customerProducts;
        }

        private void AddRow(Table table, string name, int id)
        {
            table.AddRow(name, id.ToString());
        }

        private void AddRow(Table table, string name, int id, int price, int count)
        {
            table.AddRow(name, id.ToString(), price.ToString(), count.ToString());
        }

        private void AddRow(Table table, string name, int id, int count)
        {
            table.AddRow(name, id.ToString(), count.ToString());
        }
    }
}