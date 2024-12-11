using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemDeGestiuneAlRisipei
{
    public class Driver
    {
        private int id;
        private string email;
        private string username; //do we need this?
        private string password; //do we need this?
        private string phoneNumber; //do we need this?
        private double salary;
        private int idTruck; //sau sa aiba un obiect de tip truck?
        private List<Route> listRoutes; //same here
        private bool isAvailable; //daca soferul poate primi o ruta noua sau daca are deja una care nu e inca finalizata

        private static int idCounter = 110000;

        public Driver()
        {
            idCounter++;
            this.id = idCounter;
            idTruck = 0;
            listRoutes = new List<Route>();
            isAvailable = true;
        }

        public Driver(int id, string email, string username, string password, string phoneNumber, double salary, bool isAvailable)
        {
            idCounter++;
            this.id = id;
            this.email = email;
            this.username = username;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.salary = salary;
            idTruck = 0;
            listRoutes = new List<Route>();
            this.isAvailable = isAvailable;
        }

        public Driver(string email, string username, string password, string phoneNumber, double salary)
        {
            idCounter++;
            this.id = idCounter;
            this.email = email;
            this.username = username;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.salary = salary;
            idTruck = 0;
            listRoutes = new List<Route>();
            isAvailable = true;
        }

        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public double Salary { get => salary; set => salary = value; }
        public int IdTruck { get => idTruck; set => idTruck = value; }
        public List<Route> ListRoutes { get => listRoutes; set => listRoutes = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }

        public void assignRoute(Route route)
        {
            listRoutes.Add(route);
        }
    }
}
