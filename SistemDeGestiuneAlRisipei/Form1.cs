using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Data.OleDb;

namespace SistemDeGestiuneAlRisipei
{
    public partial class Frm_login : KryptonForm
    {
        private DatabaseAccessObject dao;
        public Frm_login()
        {
            InitializeComponent();
            dao = new DatabaseAccessObject();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //testingStuff();
            if(Program.users.Count == 0)
            {
                loadFromDatabase();
            }
        }

        private void btn_signUp_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_signUp frm_SignUp = new Frm_signUp();
            frm_SignUp.ShowDialog();
            this.Close();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            //erori

            string username = tb_username.Text;

            foreach (Administrator admin in Program.admins)
            {
                if (admin.Username == username)
                {
                    this.Hide();
                    Frm_mainPageAdmin frm_MainPageAdmin = new Frm_mainPageAdmin(admin);
                    frm_MainPageAdmin.ShowDialog();
                    this.Close();
                    break;
                }
            }

            foreach (Driver driver in Program.drivers)
            {
                if (driver.Username == username)
                {
                    this.Hide();
                    Frm_mainPageDriver frm_mainPageDriver = new Frm_mainPageDriver(driver);
                    frm_mainPageDriver.ShowDialog();
                    this.Close();
                    break;
                }
            }

            foreach (AUser user in Program.users)
            {
                if (user.Username == username)
                {
                    this.Hide();
                    Frm_mainPage frm_MainPage = new Frm_mainPage(user);
                    frm_MainPage.ShowDialog();
                    this.Close();
                    break;
                }
            }
        }

        private void testingStuff()
        {
            //Product pastaDeDinti = new Product("Pasta de dinti", 0.1, 0.1, ProductCategoryEnum.IGIENA, 15);
            //Product hartieIgienica = new Product("Hartie igienica", 0.5, 0.5, ProductCategoryEnum.IGIENA, 20);
            //Product tricou = new Product("Tricou", 0.1, 0.2, ProductCategoryEnum.IMBRACAMINTE, 5);

            //Program.products.Add(pastaDeDinti);
            //Program.products.Add(hartieIgienica);
            //Program.products.Add(tricou);

            //AUser newUser1 = new Company("email", "newUser1", "parola", "adresa", "07namcartela");
            //AUser newUser2 = new Company("email", "newUser2", "parola", "adresa", "07namcartela");
            //AUser newUser3 = new Company("email", "newUser3", "parola", "adresa", "07namcartela");
            //AUser newUser4 = new Beneficiary("email", "newUser4", "parola", "adresa", "07namcartela");

            //Order order1 = new Order(newUser1.Id);
            //order1.addProduct(pastaDeDinti, 5);
            //order1.addProduct(hartieIgienica, 10);

            //Order order2 = new Order(newUser1.Id);
            //order2.addProduct(tricou, 1);

            //Program.orders.Add(order1);
            //Program.orders.Add(order2);

            //newUser1.newOrder(order1);
            //newUser1.newOrder(order2);

            //Program.users.Add(newUser1);
            //Program.users.Add(newUser2);
            //Program.users.Add(newUser3);
            //Program.users.Add(newUser4);

            //Administrator admin1 = new Administrator("email", "admin1", "kdjlsfg", "kldsgd", 3000);

            //Program.admins.Add(admin1);

            //Driver driver1 = new Driver("akjsdf", "driver1", "asndfkdfg", "aksdhks", 4000);
            //Driver driver2 = new Driver("akjsdf", "driver2", "asndfkdfg", "aksdhks", 4000);
            //Driver driver3 = new Driver("akjsdf", "driver3", "asndfkdfg", "aksdhks", 4000);

            //Program.drivers.Add(driver1);
            //Program.drivers.Add(driver2);
            //Program.drivers.Add(driver3);

            //Route route1 = new Route(20, null);
            //Route route2 = new Route(34, null);
            //Route route3 = new Route(25, null);

            //Program.routes.Add(route1);
            //Program.routes.Add(route2);
            //Program.routes.Add(route3);
        }

        private void loadFromDatabase()
        {
            try
            {
                dao.OpenConnection();
                loadProducts();
                loadOrders();

                loadAddresses();
                loadRoutes();

                loadUsers();
                loadAdmins();
                loadDrivers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                dao.CloseConnection();
            }
        }

        private void loadUsers()
        {
            OleDbCommand comanda = new OleDbCommand();
            comanda.CommandText = "SELECT * FROM utilizator";
            comanda.Connection = dao.getConnection();

            OleDbDataReader reader = comanda.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                string email = reader["Email"].ToString();
                string username = reader["Username"].ToString();
                string parola = reader["Parola"].ToString();
                double latitudineAdresa = Convert.ToDouble(reader["LatitudineAdresa"]);
                double longitudineAdresa = Convert.ToDouble(reader["LongitudineAdresa"]);
                string telefon = reader["Telefon"].ToString();

                List <Order> orders = new List<Order>();

                foreach(Order order in Program.orders)
                {
                    if(order.IdUser == id)
                    {
                        orders.Add(order);
                    }
                }

                AUser newUser;

                if (id < 140000)
                {
                    //cont companie
                    newUser = new Company(id, email, username, parola, latitudineAdresa, longitudineAdresa, telefon, orders);
                }
                else
                {
                    //cont beneficiar
                    newUser = new Beneficiary(id, email, username, parola, latitudineAdresa, longitudineAdresa, telefon, orders);
                }

                Program.users.Add(newUser);
            }
            reader.Close();
        }

        private void loadAdmins()
        {
            OleDbCommand comanda = new OleDbCommand();
            comanda.CommandText = "SELECT * FROM administrator";
            comanda.Connection = dao.getConnection();

            OleDbDataReader reader = comanda.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                string email = reader["Email"].ToString();
                string username = reader["Username"].ToString();
                string parola = reader["Parola"].ToString();
                string telefon = reader["Telefon"].ToString();
                int salariu = Convert.ToInt32(reader["Salariu"]);

                Administrator newAdmin = new Administrator(id, email, username, parola, telefon, salariu);
                Program.admins.Add(newAdmin);
            }
            reader.Close();
        }

        private void loadDrivers()
        {
            OleDbCommand comanda = new OleDbCommand();
            comanda.CommandText = "SELECT * FROM sofer";
            comanda.Connection = dao.getConnection();

            OleDbDataReader reader = comanda.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                string email = reader["Email"].ToString();
                string username = reader["Username"].ToString();
                string parola = reader["Parola"].ToString();
                string telefon = reader["Telefon"].ToString();
                int salariu = Convert.ToInt32(reader["Salariu"]);
                bool esteDisponibil = Convert.ToBoolean(reader["EsteDisponibil"]);

                Driver newDriver = new Driver(id, email, username, parola, telefon, salariu, esteDisponibil);

                foreach (Route route in Program.routes)
                {
                    if (route.IdDriver == id)
                    {
                        newDriver.assignRoute(route);
                    }
                }

                Program.drivers.Add(newDriver);
            }
            reader.Close();
        }

        private void loadProducts()
        {
            OleDbCommand comanda = new OleDbCommand();
            comanda.CommandText = "SELECT * FROM produs";
            comanda.Connection = dao.getConnection();

            OleDbDataReader reader = comanda.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                string nume = reader["Nume"].ToString();
                double greutate = Convert.ToDouble(reader["Greutate"]);
                double volum = Convert.ToDouble(reader["Volum"]);
                ProductCategoryEnum categorie = (ProductCategoryEnum) Enum.Parse(typeof(ProductCategoryEnum), reader["Categorie"].ToString());
                int nrBucati = Convert.ToInt32(reader["NrBucati"].ToString());

                Product newProduct = new Product(id, nume, greutate, volum, categorie, nrBucati);
                Program.products.Add(newProduct);
            }
            reader.Close();
        }

        private void loadOrders()
        {
            OleDbCommand comanda = new OleDbCommand();
            comanda.CommandText = "SELECT * FROM comanda";
            comanda.Connection = dao.getConnection();

            OleDbDataReader reader = comanda.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                //int administratorId = Convert.ToInt32(reader["AdministratorId"]);
                //int soferId = Convert.ToInt32(reader["SoferId"]);
                int utilizatorId = Convert.ToInt32(reader["UtilizatorID"]);
                double greutate = Convert.ToDouble(reader["Greutate"]);
                double volum = Convert.ToDouble(reader["Volum"]);
                OrderStatusEnum status = (OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), reader["Status"].ToString());
                double latitudineAdresa = Convert.ToDouble(reader["LatitudineAdresa"]);
                double longitudineAdresa = Convert.ToDouble(reader["LongitudineAdresa"]);

                Order newOrder = new Order(id, utilizatorId, greutate, volum, status, latitudineAdresa, longitudineAdresa);
                Program.orders.Add(newOrder);
            }

            reader.Close();

            comanda.CommandText = "SELECT * FROM rand_comanda";
            reader = comanda.ExecuteReader();

            //citire din rand_comanda si adaugare la comenzi

            while (reader.Read())
            {
                int comandaId = Convert.ToInt32(reader["ComandaID"]);
                int produsId = Convert.ToInt32(reader["ProdusID"]);
                int cantitate = Convert.ToInt32(reader["Cantitate"]);

                Product selectedProd = new Product();

                foreach(Product product in Program.products)
                {
                    if(product.Id == produsId)
                    {
                        selectedProd = product;
                        break;
                    }
                }

                foreach (Order order in Program.orders)
                {
                    if(order.Id == comandaId)
                    {
                        order.addProduct(selectedProd, cantitate);
                        break;
                    }
                }
            }
            
            reader.Close();
        }

        private void loadAddresses()
        {
            OleDbCommand comanda = new OleDbCommand();
            comanda.CommandText = "SELECT * FROM adresa";
            comanda.Connection = dao.getConnection();

            OleDbDataReader reader = comanda.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                double latitudineAdresa = Convert.ToDouble(reader["Latitudine"]);
                double longitudineAdresa = Convert.ToDouble(reader["Longitudine"]);

                Address newAddress = new Address(id, latitudineAdresa, longitudineAdresa);
                Program.addresses.Add(newAddress);
            }
            reader.Close();
        }

        private void loadRoutes()
        {
            OleDbCommand comanda = new OleDbCommand();
            comanda.CommandText = "SELECT * FROM ruta";
            comanda.Connection = dao.getConnection();

            OleDbDataReader reader = comanda.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                bool exists = true;
                int soferID = 0;

                try
                {
                    soferID = Convert.ToInt32(reader["SoferID"]);
                }
                catch (Exception e)
                {
                    exists = false;
                }

                double lungime = Convert.ToDouble(reader["Lungime"]);
                bool esteDistribuita = Convert.ToBoolean(reader["EsteDistribuita"]);
                RouteStatusEnum status = (RouteStatusEnum)Enum.Parse(typeof(RouteStatusEnum), reader["Status"].ToString());

                Route newRoute = new Route(id, lungime, esteDistribuita, status, soferID);
                Program.routes.Add(newRoute);
            }

            reader.Close();

            comanda.CommandText = "SELECT * FROM ruta_adresa";
            reader = comanda.ExecuteReader();

            while (reader.Read())
            {
                int rutaId = Convert.ToInt32(reader["RutaID"]);
                int adresaId = Convert.ToInt32(reader["AdresaID"]);

                Address selectedAddress = new Address();

                foreach (Address address in Program.addresses)
                {
                    if (address.Id == adresaId)
                    {
                        selectedAddress = address;
                        break;
                    }
                }

                foreach (Route route in Program.routes)
                {
                    if (route.Id == rutaId)
                    {
                        route.addAddress(selectedAddress);
                        break;
                    }
                }
            }

            reader.Close();
        }
    }
}
