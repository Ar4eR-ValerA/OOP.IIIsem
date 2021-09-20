using Shops.Entities;
using Shops.Services;
using Shops.Ui.Tools;
using Spectre.Console;

namespace Shops.Ui
{
    public class UiService
    {
        private readonly ShopManager _shopManager;
        private readonly Executor _executor;
        private readonly Asker _asker;
        private Customer _customer;

        public UiService()
        {
            _executor = new Executor();
            _shopManager = new ShopManager();
            _asker = new Asker();
        }

        public void Run()
        {
            _customer = _executor.ExecuteCreateCustomer();

            Command[] commands =
            {
                new Command(
                    "register shop",
                    () => _executor.ExecuteRegisterShop(_shopManager)),

                new Command(
                    "register product",
                    () => _executor.ExecuteRegisterCustomer(_shopManager)),

                new Command(
                    "add products to shop",
                    () => _executor.ExecuteAddProducts(_shopManager)),

                new Command(
                    "make purchase",
                    () => _executor.ExecuteMakePurchase(_shopManager, _customer)),

                new Command(
                    "show shop's products",
                    () => _executor.ExecuteShowShopProducts(_shopManager)),

                new Command(
                    "show customer's products",
                    () => _executor.ExecuteShowCustomerProducts(_customer)),

                new Command(
                    "change customer's balance",
                    () => _executor.ExecuteChangeCustomerBalance(_customer)),

                new Command("exit"),
            };

            Command command = _asker.AskChoices("Enter command", commands);

            while (command.Title != "exit")
            {
                command.Action();

                AnsiConsole.Clear();
                _executor.ExecuteShowCustomerDetails(_customer);
                _executor.ExecuteRenderMainTable(_shopManager);
                command = _asker.AskChoices("Enter command", commands);
            }
        }
    }
}