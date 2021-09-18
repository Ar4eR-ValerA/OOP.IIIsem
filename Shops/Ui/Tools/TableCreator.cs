using Shops.Entities;
using Spectre.Console;

namespace Shops.Ui.Tools
{
    public class TableCreator
    {
        public (Table, Table, Table) MainTable()
        {
            var mainTable = new Table();
            var shopsTable = new Table();
            var productsTable = new Table();

            mainTable.AddColumn("Shops");
            mainTable.AddColumn("Products");
            mainTable.AddRow(shopsTable, productsTable);

            shopsTable.AddColumn("Name:");
            shopsTable.AddColumn("id:");
            shopsTable.Border = TableBorder.None;

            productsTable.AddColumn("Name:");
            productsTable.AddColumn("id:");
            productsTable.Border = TableBorder.None;

            return (mainTable, shopsTable, productsTable);
        }

        public Table ShopPersonalTable(Shop shop)
        {
            var shopTable = new Table
            {
                Title = new TableTitle($"{shop.Name} id: {shop.Id}"),
            };

            shopTable.AddColumns("Product name", "ID", "Price", "Count");

            return shopTable;
        }

        public Table CustomerPersonalTable(Customer customer)
        {
            var customerTable = new Table
            {
                Title = new TableTitle($"{customer.Name}: {customer.Balance}\n"),
            };
            customerTable.AddColumns("Product name", "ID", "Count");

            return customerTable;
        }
    }
}