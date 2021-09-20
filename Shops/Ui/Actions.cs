using System.Collections.Generic;
using Shops.Entities;
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

            (mainTable, shopsTable, productsTable) = _tableCreator.CreateMainTable();
            _tableFiller.FillMainTable(shopsTable, productsTable, shopManager);

            AnsiConsole.Render(mainTable);
        }

        public void RegisterShop(ShopManager shopManager, string title)
        {
            shopManager.RegisterShop(new Shop(title));
        }

        public void RegisterProduct(ShopManager shopManager, string title)
        {
            shopManager.RegisterProduct(new Product(title));
        }

        public void AddProducts(ShopManager shopManager, int shopId, int productId, int productCount, int productPrice)
        {
            var shopProduct = new ShopProductDetails(shopManager.Products[productId], productCount, productPrice);
            shopManager.Shops[shopId].AddProduct(shopProduct);
        }

        public void MakePurchase(
            Customer customer,
            List<CustomerProductDetails> customerShoppingList,
            Shop chosenShop)
        {
            chosenShop.Purchase(customer, customerShoppingList);
        }

        public void ShowShopProducts(ShopManager shopManager, int shopId)
        {
            AnsiConsole.Clear();

            Table shopTable = _tableCreator.CreateShopPersonalTable(shopManager.Shops[shopId]);
            _tableFiller.FillShopPersonalTable(shopTable, shopManager.Shops[shopId]);

            AnsiConsole.Render(shopTable);

            _asker.AskExit(string.Empty);
        }

        public Customer CreateCustomer(string name, int balance)
        {
            return new Customer(balance, name);
        }

        public void ShowCustomerDetails(Customer customer)
        {
            AnsiConsole.Write($"{customer.Name}: {customer.Balance}$\n");
        }

        public void ChangeCustomerBalance(Customer customer, int newBalance)
        {
            customer.Balance = newBalance;
        }

        public void ShowCustomerProducts(Customer customer)
        {
            AnsiConsole.Clear();

            Table customerTable = _tableCreator.CreateCustomerPersonalTable(customer);
            _tableFiller.FillCustomerPersonalTable(customerTable, customer);

            AnsiConsole.Render(customerTable);

            _asker.AskExit(string.Empty);
        }
    }
}