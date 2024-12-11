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
    public partial class Frm_mainPage : KryptonForm
    {
        private AUser user;
        private DatabaseAccessObject dao;
        public Frm_mainPage()
        {
            InitializeComponent();
            dao = new DatabaseAccessObject();
        }

        public Frm_mainPage(AUser user):this()
        {
            this.user = user;
        }
        private void Frm_mainPage_Load(object sender, EventArgs e)
        {
            lbl_2.Text = user.Username;
            loadOrders();
        }

        private void loadOrders()
        {
            List<Order> ordersList = user.Orders;
            lv_orders.Items.Clear();

            foreach (Order order in ordersList)
            {
                ListViewItem item = new ListViewItem(order.Id.ToString());
                item.SubItems.Add(order.Status.ToString());
                lv_orders.Items.Add(item);
            }

            lv_orders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv_orders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_login frm_login = new Frm_login();
            frm_login.ShowDialog();
            this.Close();
        }

        private void btn_newOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_newOrder frm_newOrder = new Frm_newOrder(this.user);
            frm_newOrder.ShowDialog();
            this.Close();
        }

        private void btn_modifyOrder_Click(object sender, EventArgs e)
        {
            //daca comanda este unverified!!!
            Order selectedOrder = new Order();

            foreach (ListViewItem item in lv_orders.Items)
            {
                if (item.Selected)
                {
                    int orderId = Int32.Parse(item.SubItems[0].Text);
                    List<Order> ordersList = user.Orders;
                    foreach (Order order in ordersList)
                    {
                        if (order.Id == orderId)
                        {
                            if(order.Status == OrderStatusEnum.UNVERIFIED)
                            {
                                selectedOrder = order;
                                this.Hide();
                                Frm_newOrder frm_newOrder = new Frm_newOrder(this.user, selectedOrder);
                                frm_newOrder.ShowDialog();
                                this.Close();
                                break;
                            }
                        }
                    }
                    
                }
            }
            
        }

        public void showOrderItems()
        {
            //la dublu click pe o comanda se afiseaza produsele din comanda respectiva
            lv_products.Items.Clear();

            foreach (ListViewItem item in lv_orders.Items)
            {
                if (item.Selected) //cautam comanda care a fost selectata
                {
                    int orderId = Int32.Parse(item.SubItems[0].Text);
                    List<Order> ordersList = user.Orders;

                    List<Product> products = new List<Product>();
                    List<int> noProducts = new List<int>();

                    foreach (Order order in ordersList)
                    {
                        if (order.Id == orderId)
                        {
                            products = order.Products; //preluam produsele din comanda respectiva
                            noProducts = order.NoProducts;
                        }
                    }

                    foreach (var tuple in products.Zip(noProducts, (x, y) => (product: x, noProducts: y)))//afisam fiecare produs in list view
                    {
                        ListViewItem newProduct = new ListViewItem(tuple.product.Id.ToString());

                        newProduct.SubItems.Add(tuple.product.Name);
                        newProduct.SubItems.Add(tuple.product.Category.ToString());

                        newProduct.SubItems.Add(tuple.noProducts.ToString()); //nr prod

                        lv_products.Items.Add(newProduct);
                    }
                }
            }

            lv_products.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv_products.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void lv_orders_ItemActivate(object sender, EventArgs e)
        {
            showOrderItems();
        }

        private void btn_deleteOrder_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lv_orders.Items)
            {
                if (item.Selected)
                {
                    int orderId = Int32.Parse(item.SubItems[0].Text);

                    List<Order> ordersList = user.Orders;
                    foreach (Order order in ordersList)
                    {
                        if (order.Id == orderId)
                        {
                            if(order.Status == OrderStatusEnum.UNVERIFIED)
                            {
                                Program.orders.Remove(order);
                                user.deleteOrder(order);
                                deleteFromDatabase(orderId);
                                loadOrders();
                                showOrderItems();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btn_verifyOrder_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lv_orders.Items)
            {
                if (item.Selected)
                {
                    int orderId = Int32.Parse(item.SubItems[0].Text);

                    List<Order> ordersList = user.Orders;
                    foreach (Order order in ordersList)
                    {
                        if (order.Id == orderId)
                        {
                            user.confirmOrder(order);
                            updateDatabase(orderId);
                            loadOrders();
                            break;
                        }
                    }
                }
            }
        }

        private void deleteFromDatabase(int id)
        {
            try
            {
                dao.OpenConnection();
                OleDbCommand comanda = new OleDbCommand();
                comanda.Connection = dao.getConnection();
                comanda.CommandText = "DELETE FROM comanda WHERE ID=" + id;
                comanda.ExecuteNonQuery();

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

        private void updateDatabase(int id)
        {
            try
            {
                dao.OpenConnection();
                OleDbCommand comanda = new OleDbCommand();
                comanda.Connection = dao.getConnection();
                comanda.CommandText = "UPDATE comanda SET status='" + "VERIFIED" + "' WHERE ID=" + id;
                comanda.ExecuteNonQuery();

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
