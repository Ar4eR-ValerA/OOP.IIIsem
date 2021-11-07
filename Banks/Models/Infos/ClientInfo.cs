using Banks.Tools;

namespace Banks.Models.Infos
{
    public class ClientInfo
    {
        private string _name;
        private string _surname;
        private string _address;

        public ClientInfo(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public ClientInfo(string name, string surname, string address, int passport)
        {
            Name = name;
            Surname = surname;
            Address = address;
            Passport = passport;
        }

        public string Name
        {
            get => _name;
            set => _name = value ?? throw new BanksException("Name is null");
        }

        public string Surname
        {
            get => _surname;
            set => _surname = value ?? throw new BanksException("Surname is null");
        }

        public string Address
        {
            get => _address;
            set => _address = value ?? throw new BanksException("Address is null");
        }

        public int Passport { get; set; }
    }
}