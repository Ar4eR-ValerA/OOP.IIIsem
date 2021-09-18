using Shops.Entities;
using Shops.Services;
using Shops.Ui.Tools;
using Spectre.Console;

namespace Shops.Ui
{
    public class UiService
    {
        private ShopManager _shopManager;
        private Actions _actions;
        private Asker _asker;
        private Customer _customer;

        public UiService()
        {
            _actions = new Actions();
            _shopManager = new ShopManager();
            _asker = new Asker();
        }

        public void Run()
        {
            _customer = _actions.CreateCustomer();

            Command[] commands =
            {
                new Command("register shop", () => _actions.RegisterShop(_shopManager)),
                new Command("register product", () => _actions.RegisterProduct(_shopManager)),
                new Command("add products to shop", () => _actions.AddProducts(_shopManager)),
                new Command("make purchase", () => _actions.MakePurchase(_customer, _shopManager)),
                new Command("show shop's products", () => _actions.ShowShopProducts(_shopManager)),
                new Command("show customer's products", () => _actions.ShowCustomerProducts(_customer)),
                new Command("change customer's balance", () => _actions.ChangeCustomerBalance(_customer)),
                new Command("exit", AnsiConsole.Clear),
            };

            Command command = _asker.AskChoices("Enter command", commands);

            while (command.Title != "exit")
            {
                command.Action();

                AnsiConsole.Clear();
                _actions.ShowCustomerDetails(_customer);
                _actions.RenderMainTable(_shopManager);
                command = _asker.AskChoices("Enter command", commands);
            }
        }
    }
}