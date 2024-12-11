using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemDeGestiuneAlRisipei
{
    internal static class Program
    {
        public static List<AUser> users = new List<AUser>();

        public static List<Order> orders = new List<Order>(); //liste separate pentru comenziC si comenziB

        public static List<Product> products = new List<Product>();

        public static List<Administrator> admins = new List<Administrator>();

        public static List<Driver> drivers = new List<Driver>();

        public static List<Truck> trucks = new List<Truck>();

        public static List<Route> routes = new List<Route>();

        public static List<Address> addresses = new List<Address>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm_login());
        }
    }
}
