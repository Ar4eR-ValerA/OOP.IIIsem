using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;
using Shops.Services;
using Spectre.Console;

namespace Shops.Ui.Tools
{
    public class Asker
    {
        public T AskChoices<T>(string message, IEnumerable<T> choices)
        {
            IEnumerable<T> enumerable = choices.ToList();
            if (!enumerable.Any())
            {
                throw new ShopsException("There are no objects for choice");
            }

            AnsiConsole.Write(message + "\n\n");
            return AnsiConsole.Prompt(new SelectionPrompt<T>()
                .AddChoices(enumerable));
        }

        public string AskChoices(string message, string choice)
        {
            if (!choice.Any())
            {
                throw new ShopsException("There are no objects for choice");
            }

            AnsiConsole.Write(message + "\n\n");
            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .AddChoices(choice));
        }

        public void AskExit(string message)
        {
            AnsiConsole.Write(message + "\n\n");
            AnsiConsole.Prompt(new SelectionPrompt<string>()
                .AddChoices("exit"));
        }

        public int AskInt(string message)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>(message + "\n")
                    .Validate(value =>
                    {
                        return value switch
                        {
                            < 0 => ValidationResult.Error("[red]Value must be positive[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));
        }

        public string AskString(string message)
        {
            return AnsiConsole.Ask<string>(message + "\n");
        }

        public List<CustomerProductDetails> AskShoppingList(ShopManager shopManager)
        {
            AnsiConsole.Write("Enter id of product\n\n");

            var customerProducts = new List<CustomerProductDetails>();

            string next = "next";
            while (next != "stop")
            {
                int productId = AskChoices(
                    "Enter id of product:",
                    shopManager.Products.Keys);
                int productCount = AskInt("Enter amount of this product\n");

                customerProducts.Add(new CustomerProductDetails(shopManager.Products[productId], productCount));

                next = AskChoices(
                    "Another product or enough?",
                    new[] { "next", "stop" });
            }

            return customerProducts;
        }

        public Shop AskShopForPurchase(List<CustomerProductDetails> customerShoppingList, ShopManager shopManager)
        {
            string shopChoice = AskChoices(
                "Special shop or the cheapest?",
                new[] { "cheapest", "special" });

            if (shopChoice == "cheapest")
            {
                return shopManager.FindCheapestShop(customerShoppingList);
            }

            IReadOnlyList<Shop> shops = shopManager.FindShops(customerShoppingList);
            int shopId = AskChoices("Enter id of shop", shops.Select(shop => shop.Id).ToList());

            return shopManager.Shops[shopId];
        }
    }
}