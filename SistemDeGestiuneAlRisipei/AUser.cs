using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemDeGestiuneAlRisipei
{
    public abstract class AUser
    {
        private int id;
        private string email;
        private string username;
        private string password;
        private double latitudineAdresa;
        private double longitudineAdresa;
        private string phoneNumber;
        private List<Order> orders;
        public AUser()
        {
            this.email = "";
            this.username = "";
            this.password = "";
            this.latitudineAdresa = 0;
            this.longitudineAdresa = 0;
            this.phoneNumber = "";
            this.orders = new List<Order>();
        }

        public AUser(int id, string email, string username, string password, double longitudineAdresa, double latitudineAdresa, string phoneNumber, List<Order> orders)
        {
            this.id = id;
            this.email = email;
            this.username = username;
            this.password = password;
            this.latitudineAdresa = latitudineAdresa;
            this.longitudineAdresa = longitudineAdresa;
            this.phoneNumber = phoneNumber;
            this.orders = orders;
        }

        public AUser(string email, string username, string password, string phoneNumber)
        {
            this.email = email;
            this.username = username;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.orders = new List<Order>();
        }

        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public double LatitudineAdresa { get => latitudineAdresa; set => latitudineAdresa = value; }
        public double LongitudineAdresa { get => longitudineAdresa; set => longitudineAdresa = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public List<Order> Orders { get => orders; set => orders = value; }

        public abstract void newOrder(Order order);
        public abstract void modifyOrder(Order order);
        public abstract void deleteOrder(Order order);
        public abstract void confirmOrder(Order order);
    }
}
