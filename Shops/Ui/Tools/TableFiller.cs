using Shops.Entities;
using Shops.Models;
using Shops.Services;
using Spectre.Console;

namespace Shops.Ui.Tools
{
    public class TableFiller
    {
        public void FillMainTable(Table shopsTable, Table productsTable, ShopManager shopManager)
        {
            foreach (Shop shop in shopManager.Shops.Values)
            {
                shopsTable.AddRow(shop.Name, shop.Id.ToString());
            }

            foreach (Product product in shopManager.Products.Values)
            {
                productsTable.AddRow(product.Name, product.Id.ToString());
            }
        }

        public void FillShopPersonalTable(Table shopTable, Shop shop)
        {
            foreach (ShopProductDetails product in shop.ProductsDetailsList)
            {
                shopTable.AddRow(
                    product.Product.ToString(),
                    product.Product.Id.ToString(),
                    product.Price.ToString(),
                    product.Count.ToString());
            }
        }

        public void FillCustomerPersonalTable(Table customerTable, Customer customer)
        {
            foreach (CustomerProductDetails product in customer.ProductList)
            {
                customerTable.AddRow(
                    product.Product.ToString(),
                    product.Product.Id.ToString(),
                    product.Count.ToString());
            }
        }
    }
}