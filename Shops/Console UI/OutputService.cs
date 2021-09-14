using System.Collections.Generic;
using Shops.Entities;
using Shops.Services;
using Spectre.Console;

namespace Shops.Console_UI
{
    public class OutputService
    {
        private ShopManager _shopManager;
        private Actions _actions;
        private Asking _asking;
        private Customer _customer;

        public OutputService()
        {
            _actions = new Actions();
            _shopManager = new ShopManager();
            _asking = new Asking();
        }

        public void MainService()
        {
            _customer = _actions.CreateCustomer();

            IEnumerable<string> commands = new[]
            {
                "register shop",
                "register product",
                "add products to shop",
                "make purchase",
                "show shop's products",
                "show customer's products",
                "change customer's balance",
                "exit",
            };

            string command = _asking.AskChoices("Enter command", commands);

            while (command != "exit")
            {
                switch (command)
                {
                    case "register shop":
                        _actions.RegisterShop(_shopManager);
                        break;

                    case "register product":
                        _actions.RegisterProduct(_shopManager);
                        break;

                    case "add products to shop":
                        _actions.AddProducts(_shopManager);
                        break;

                    case "show shop's products":
                        _actions.ShowShopProducts(_shopManager);
                        break;

                    case "make purchase":
                        _actions.MakePurchase(_customer, _shopManager);
                        break;

                    case "change customer's balance":
                        _actions.ChangeCustomerBalance(_customer);
                        break;

                    case "show customer's products":
                        _actions.ShowCustomerProducts(_customer);
                        break;
                }

                AnsiConsole.Clear();
                _actions.ShowCustomerDetails(_customer);
                _actions.RenderMainTable(_shopManager);
                command = _asking.AskChoices("Enter command", commands);
            }
        }
    }
}