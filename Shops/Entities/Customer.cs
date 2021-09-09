using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        private int _balance;

        public Customer(int balance, string name)
        {
            if (balance < 0)
            {
                throw new ShopsException($"Negative customer's balance: {_balance}");
            }

            _balance = balance;
            Name = name;
        }

        public int Balance
        {
            get => _balance;
            internal set
            {
                if (value < 0)
                {
                    throw new ShopsException($"Negative customer's balance: {_balance}");
                }

                _balance = value;
            }
        }

        public string Name { get; }
    }
}