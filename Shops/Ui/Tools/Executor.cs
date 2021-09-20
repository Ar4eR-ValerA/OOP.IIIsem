using System.Collections.Generic;
using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;
using Shops.Services;

namespace Shops.Ui.Tools
{
    public class Executor
    {
        private readonly Inputter _inputter;
        private readonly Asker _asker;
        private readonly Actions _actions;

        public Executor()
        {
            _inputter = new Inputter();
            _actions = new Actions();
            _asker = new Asker();
        }

        public Customer ExecuteCreateCustomer()
        {
            try
            {
                return _actions.CreateCustomer(_inputter.InputName(), _inputter.InputBalance());
            }
            catch (ShopsException exception)
            {
                _asker.AskExit(exception.Message);
                return _actions.CreateCustomer("Error", 0);
            }
        }

        public void ExecuteRegisterShop(ShopManager shopManager)
        {
            try
            {
                _actions.RegisterShop(shopManager, _inputter.InputName());
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }

        public void ExecuteRegisterCustomer(ShopManager shopManager)
        {
            try
            {
                _actions.RegisterProduct(shopManager, _inputter.InputName());
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }

        public void ExecuteAddProducts(ShopManager shopManager)
        {
            try
            {
                _actions.AddProducts(
                    shopManager,
                    _inputter.InputShopId(shopManager.Shops.Keys),
                    _inputter.InputProductId(shopManager.Products.Keys),
                    _inputter.InputProductCount(),
                    _inputter.InputBalance());
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }

        public void ExecuteMakePurchase(ShopManager shopManager, Customer customer)
        {
            try
            {
                List<CustomerProductDetails> shoppingList = _inputter.InputShoppingList(shopManager.Products);
                _actions.MakePurchase(
                    customer,
                    shoppingList,
                    _inputter.InputShopForPurchase(shoppingList, shopManager));
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }

        public void ExecuteShowShopProducts(ShopManager shopManager)
        {
            try
            {
                _actions.ShowShopProducts(
                    shopManager,
                    _inputter.InputShopId(shopManager.Shops.Keys));
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }

        public void ExecuteShowCustomerProducts(Customer customer)
        {
            try
            {
                _actions.ShowCustomerProducts(customer);
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }

        public void ExecuteChangeCustomerBalance(Customer customer)
        {
            try
            {
                _actions.ChangeCustomerBalance(customer, _inputter.InputBalance());
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }

        public void ExecuteRenderMainTable(ShopManager shopManager)
        {
            try
            {
                _actions.RenderMainTable(shopManager);
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }

        public void ExecuteShowCustomerDetails(Customer customer)
        {
            try
            {
                _actions.ShowCustomerDetails(customer);
            }
            catch (ShopsException shopsException)
            {
                _asker.AskExit(shopsException.Message);
            }
        }
    }
}