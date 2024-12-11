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
using Microsoft.Scripting.Hosting;
using System.Data.OleDb;

namespace SistemDeGestiuneAlRisipei
{
    public partial class Frm_mainPageAdmin : KryptonForm
    {
        private DatabaseAccessObject dao;
        private Administrator admin;
        private Route newRoute;
        private List<Order> verifiedOrders;
        
        public Frm_mainPageAdmin()
        {
            InitializeComponent();
            dao = new DatabaseAccessObject();
            verifiedOrders = new List<Order>();        }

        public Frm_mainPageAdmin(Administrator admin):this()
        {
            this.admin = admin;
        }

        private void Frm_mainPageAdmin_Load(object sender, EventArgs e)
        {
            lbl_2.Text = admin.Username;
            loadOrders();
            loadRoutes();
            loadDrivers();
        }

        private void loadFromDatabase()
        {
            string connString = @"Provider = Microsoft.ACE.OLEDB.16.0; Data Source = E:/Facultate/Licenta/BazaDeDate/Test2DB.accdb;";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.16.0; Data Source = E:/Facultate/Licenta/BazaDeDate/SistemGestiuneDB.accdb;"))
                {
                    connection.Open();
                    Console.WriteLine("Connection successful!");

                    //citire din db
                    OleDbCommand comanda = new OleDbCommand();
                    comanda.CommandText = "SELECT * FROM utilizator";
                    comanda.Connection = connection;

                    OleDbDataReader reader = comanda.ExecuteReader();
                    while (reader.Read())
                    {
                        ListViewItem itm = new ListViewItem(reader["ID"].ToString());
                        itm.SubItems.Add(reader["nume"].ToString());
                        lv_orders.Items.Add(itm);
                    }
                    reader.Close();

                    //inserare in db

                    comanda.CommandText = "INSERT INTO utilizator VALUES(?,?)";
                    comanda.Parameters.Add("ID", OleDbType.Integer).Value = 101;
                    comanda.Parameters.Add("nume", OleDbType.Char, 30).Value = "Georgica";
                    comanda.ExecuteNonQuery();
                }
            }
            catch (System.TypeInitializationException ex)
            {
                Console.WriteLine("Type Initialization Exception: " + ex.InnerException?.Message);
                Console.WriteLine(ex.InnerException?.Message);
                Console.WriteLine(ex.InnerException?.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void loadOrders()
        {
            List<Order> ordersList = Program.orders;
            lv_orders.Items.Clear();

            foreach (Order order in ordersList)
            {
                if (order.Status == OrderStatusEnum.VERIFIED)
                {
                    verifiedOrders.Add(order);
                    ListViewItem item = new ListViewItem(order.Id.ToString());
                    item.SubItems.Add(order.IdUser.ToString());
                    item.SubItems.Add(order.Status.ToString());
                    lv_orders.Items.Add(item);
                }
            }

            lv_orders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv_orders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void loadRoutes()
        {
            List<Route> routesList = Program.routes;
            lv_routes.Items.Clear();

            foreach (Route route in routesList)
            {
                ListViewItem item = new ListViewItem(route.Id.ToString());
                item.SubItems.Add(route.TotalLength.ToString());
                item.SubItems.Add(route.RouteStatus.ToString());
                item.SubItems.Add(route.IsAssigned.ToString());

                lv_routes.Items.Add(item);
            }

            lv_routes.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv_routes.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void loadDrivers()
        {
            List<Driver> driversList = Program.drivers; //aici ar trb sa preluam numai comenzile confirmate sau mai departe (cred)
            lv_drivers.Items.Clear();

            foreach (Driver driver in driversList)
            {
                ListViewItem item = new ListViewItem(driver.Id.ToString());

                if (driver.IdTruck == 0)
                {
                    item.SubItems.Add("no truck");
                }
                else
                {
                    item.SubItems.Add(driver.IdTruck.ToString());
                }

                if (driver.IsAvailable == true)
                {
                    item.SubItems.Add("True");
                }
                else
                {
                    item.SubItems.Add("False");
                }

                lv_drivers.Items.Add(item);
            }

            lv_drivers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv_drivers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_login frm_login = new Frm_login();
            frm_login.ShowDialog();
            this.Close();
        }

        private void btn_calculateRoute_Click(object sender, EventArgs e)
        {
            if(this.admin.calculateRoute(verifiedOrders) != null)
            {
                newRoute = this.admin.calculateRoute(verifiedOrders);
                label1.Text = "Ruta calculata are lungimea de: " + newRoute.TotalLength.ToString() + " km.";
            }
        }

        private void btn_assignRoute_Click(object sender, EventArgs e)
        {
            //if there is selected route, assign it to a driver
            Route selectedRoute = new Route();

            foreach(ListViewItem item in lv_routes.Items)
            {
                if(item.Selected)
                {
                    int routeId = Int32.Parse(item.SubItems[0].Text);

                    foreach(Route route in Program.routes)
                    {
                        if(route.Id == routeId)
                        {
                            selectedRoute = route;
                        }
                    }
                }
            }

            if(selectedRoute.RouteStatus != RouteStatusEnum.UNASSIGNED)
            {
                return;
            }

            //look for available driver
            Driver availableDriver = new Driver();

            foreach(Driver driver in Program.drivers)
            {
                if (driver.IsAvailable)
                {
                    driver.assignRoute(selectedRoute);

                    selectedRoute.IsAssigned = true;
                    selectedRoute.RouteStatus = RouteStatusEnum.ASSIGNED;
                    driver.IsAvailable = false;

                    try
                    {
                        dao.OpenConnection();
                        OleDbCommand comanda = new OleDbCommand();
                        comanda.Connection = dao.getConnection();
                        comanda.CommandText = "UPDATE ruta SET status='" + "ASSIGNED" + "', esteDistribuita=" + 1 +" WHERE ID=" + selectedRoute.Id;
                        comanda.ExecuteNonQuery();

                        OleDbCommand comanda2 = new OleDbCommand();
                        comanda2.Connection = dao.getConnection();
                        comanda2.CommandText = "UPDATE sofer SET esteDisponibil=" + 0 + " WHERE ID=" + driver.Id;
                        comanda2.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: " + ex.Message);
                    }
                    finally
                    {
                        dao.CloseConnection();
                    }
                    break;
                }
            }

            loadRoutes();
            loadDrivers();
        }

        private void btn_confirmRoute_Click(object sender, EventArgs e)
        {
            Program.routes.Add(newRoute);
            addRouteToDatabase(newRoute);


            loadOrders();
            loadRoutes();

            label1.Text = "Route details here";
        }

        private void addRouteToDatabase(Route route)
        {
            if(route == null)
            {
                return;
            }
            try
            {
                dao.OpenConnection();
                OleDbCommand comanda = new OleDbCommand();
                comanda.Connection = dao.getConnection();

                comanda.CommandText = "INSERT INTO ruta (ID, AdministratorID, Lungime, EsteDistribuita, Status) VALUES(?,?,?,?,?)";
                comanda.Parameters.Add("ID", OleDbType.Integer).Value = route.Id;
                comanda.Parameters.Add("AdministratorID", OleDbType.Integer).Value = this.admin.Id;
                comanda.Parameters.Add("Lungime", OleDbType.Double, 30).Value = route.TotalLength;
                comanda.Parameters.Add("EsteDistribuita", OleDbType.Integer).Value = route.IsAssigned;
                comanda.Parameters.Add("Status", OleDbType.Char, 30).Value = route.RouteStatus;
                comanda.ExecuteNonQuery();

                //adaugare ruta_adresa

                //OleDbCommand comanda2 = new OleDbCommand();
                //comanda2.Connection = dao.getConnection();

                //List<Tuple<double, double>> listStops = new List<Tuple<double, double>>();
                //listStops = route.ListStops;
                //Console.WriteLine("Nr de intrari noi pt baza de date: " + listStops.Count.ToString());
                //int idAddress = 0;

                //int cnt = 0;

                //foreach(Tuple<double, double> stop in listStops)
                //{
                //    foreach(Address address in Program.addresses)
                //    {
                //        if(address.LatitudineAdresa == stop.Item1 && address.LongitudineAdresa == stop.Item2)
                //        {
                //            Console.WriteLine("Intrarea nr. " + cnt.ToString());
                //            cnt++;

                //            idAddress = address.Id;

                //            comanda2.CommandText = "SELECT MAX(ID) FROM ruta_adresa";
                //            int id = Convert.ToInt32(comanda2.ExecuteScalar());

                //            comanda2.CommandText = "INSERT INTO ruta_adresa (ID, RutaID, AdresaID) VALUES(?,?,?)";
                //            comanda2.Parameters.Add("ID", OleDbType.Integer).Value = id + 1;
                //            comanda2.Parameters.Add("RutaID", OleDbType.Integer).Value = route.Id;
                //            comanda2.Parameters.Add("AdresaID", OleDbType.Integer).Value = idAddress;
                //            comanda2.ExecuteNonQuery();
                //        }
                //    }
                //}

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
    }
}
