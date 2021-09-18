using System.Collections.Generic;
using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;
using Shops.Services;
using Shops.Ui.Tools;
using Spectre.Console;

namespace Shops.Ui
{
    public class Actions
    {
        private readonly Asker _asker;
        private readonly TableCreator _tableCreator;
        private readonly TableFiller _tableFiller;

        public Actions()
        {
            _tableFiller = new TableFiller();
            _tableCreator = new TableCreator();
            _asker = new Asker();
        }

        public void RenderMainTable(ShopManager shopManager)
        {
            Table mainTable;
            Table shopsTable;
            Table productsTable;

            (mainTable, shopsTable, productsTable) = _tableCreator.MainTable();
            _tableFiller.MainTable(shopsTable, productsTable, shopManager);

            AnsiConsole.Render(mainTable);
        }

        public void RegisterShop(ShopManager shopManager)
        {
            string title = _asker.AskString("Enter the title:\n");
            shopManager.RegisterShop(title);
        }

        public void RegisterProduct(ShopManager shopManager)
        {
            string title = _asker.AskString("Enter the title:\n");
            shopManager.RegisterProduct(title);
        }

        public void AddProducts(ShopManager shopManager)
        {
            try
            {
                int shopId = _asker.AskChoices(
                    "Enter id of shop:",
                    shopManager.Shops.Keys);

                int productId = _asker.AskChoices(
                    "Enter id of product:",
                    shopManager.Products.Keys);

                int productCount = _asker.AskInt("Enter amount of this product:\n");
                int productPrice = _asker.AskInt("Enter price of this product:\n");

                var shopProduct = new ShopProductDetails(shopManager.Products[productId], productCount, productPrice);
                shopManager.Shops[shopId].AddProduct(shopProduct);
            }
            catch (ShopsException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void MakePurchase(Customer customer, ShopManager shopManager)
        {
            try
            {
                List<CustomerProductDetails> customerShoppingList =
                    _asker.AskShoppingList(shopManager);

                Shop chosenShop = _asker.AskShopForPurchase(customerShoppingList, shopManager);
                chosenShop.Purchase(customer, customerShoppingList);
            }
            catch (ShopsException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ShowShopProducts(ShopManager shopManager)
        {
            try
            {
                AnsiConsole.Clear();

                int shopId = _asker.AskChoices("Enter id of shop", shopManager.Shops.Keys);

                Table shopTable = _tableCreator.ShopPersonalTable(shopManager.Shops[shopId]);
                _tableFiller.ShopPersonalTable(shopTable, shopManager.Shops[shopId]);

                AnsiConsole.Render(shopTable);

                _asker.AskExit(string.Empty);
            }
            catch (ShopsException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public Customer CreateCustomer()
        {
            string customerName = _asker.AskString("Enter your name:\n");

            int customerBalance = _asker.AskInt("Enter your balance:\n");
            return new Customer(customerBalance, customerName);
        }

        public void ShowCustomerDetails(Customer customer)
        {
            AnsiConsole.Write($"{customer.Name}: {customer.Balance}$\n");
        }

        public void ChangeCustomerBalance(Customer customer)
        {
            int customerBalance = _asker.AskInt("Enter your new balance:\n");
            customer.Balance = customerBalance;
        }

        public void ShowCustomerProducts(Customer customer)
        {
            AnsiConsole.Clear();

            Table customerTable = _tableCreator.CustomerPersonalTable(customer);
            _tableFiller.CustomerPersonalTable(customerTable, customer);

            AnsiConsole.Render(customerTable);

            _asker.AskExit(string.Empty);
        }
    }
}