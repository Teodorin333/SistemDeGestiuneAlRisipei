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
    public partial class Frm_newOrder : KryptonForm
    {
        private AUser user;
        private Order order;
        bool modifiedOrder = false;

        private DatabaseAccessObject dao;
        public Frm_newOrder()
        {
            InitializeComponent();
            dao = new DatabaseAccessObject();
        }

        public Frm_newOrder(AUser user) : this()
        {
            this.user = user;
        }

        public Frm_newOrder(AUser user, Order order) : this()
        {
            this.user = user;
            this.order = order;
            modifiedOrder = true;
        }

        private void Frm_newOrder_Load(object sender, EventArgs e)
        {
            showComboBoxOptions();
            if(modifiedOrder)
            {
                loadExistingOrder();
                
            }
            else
            {
                order = new Order();
                order.IdUser = this.user.Id;
                order.Address = new Tuple<double, double>(this.user.LatitudineAdresa, this.user.LongitudineAdresa);
            }
        }

        private void loadExistingOrder()
        {
            lv_products.Items.Clear();
            List<Product> products = new List<Product>();
            List<int> noProducts = new List<int>();
            products = order.Products;
            noProducts = order.NoProducts;

            foreach (var tuple in products.Zip(noProducts, (x, y) => (product: x, noProducts: y)))//afisam fiecare produs in list view
            {
                ListViewItem newProduct = new ListViewItem(tuple.product.Id.ToString());

                newProduct.SubItems.Add(tuple.product.Name);
                newProduct.SubItems.Add(tuple.product.Category.ToString());

                newProduct.SubItems.Add(tuple.noProducts.ToString());

                lv_products.Items.Add(newProduct);
            }
        }

        private void showComboBoxOptions()
        {
            foreach (Product product in Program.products)
            {
                cb_products.Items.Add(product.Name);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(modifiedOrder)
            {
                user.modifyOrder(order);
            }
            else
            {
                user.newOrder(order);
                insertIntoDatabase();
            }

            this.Hide();
            Frm_mainPage frm_mainPage = new Frm_mainPage(user);
            frm_mainPage.ShowDialog();
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_mainPage frm_mainPage = new Frm_mainPage(user);
            frm_mainPage.ShowDialog();
            this.Close();
        }

        private void btn_addProduct_Click(object sender, EventArgs e)
        {
            //erori

            string userType; //trebuie sa stim tipul utilizatorului ca sa stim daca adaugam sau scoatem din stoc
            if (user.GetType() == typeof(Company)) userType = "company";
            else userType = "beneficiary";

            try
            {
                Product selectedProduct = new Product();
                foreach (Product product in Program.products)
                {
                    if (product.Name == cb_products.SelectedItem.ToString())
                    {
                        selectedProduct = product;
                    }
                }

                int noProducts = Convert.ToInt32(tb_noItems.Text);

                //verificam daca produsele cerute sunt in stoc

                if (userType == "beneficiary" && selectedProduct.NoItems < noProducts)
                {
                    MessageBox.Show("Numarul cerut de produse nu este in stoc.");
                }
                else
                {
                    //errP_newOrder.Clear();

                    if (userType == "beneficiary") selectedProduct.pullFromStock(noProducts); //only pull from stock when order is saved!!!!
                    else selectedProduct.addToStock(noProducts);

                    order.addProduct(selectedProduct, noProducts);
                    //mareste greutatea si volumul comenzii

                    ListViewItem item = new ListViewItem(selectedProduct.Id.ToString());

                    item.SubItems.Add(selectedProduct.Name);
                    item.SubItems.Add(selectedProduct.Category.ToString());

                    item.SubItems.Add(noProducts.ToString());

                    lv_products.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_eraseElem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lv_products.Items)
            {
                if (item.Selected)
                {
                    int productId = Int32.Parse(item.SubItems[0].Text);

                    List<Product> products = order.Products;

                    foreach (Product product in products)
                    {
                        if (product.Id == productId)
                        {
                            order.removeProduct(product);
                            loadExistingOrder();
                            break;
                        }
                    }
                }
            }
        }

        private void btn_verify_Click(object sender, EventArgs e)
        {
            user.confirmOrder(order);
            if (modifiedOrder)
            {
                user.modifyOrder(order);
            }
            else
            {
                user.newOrder(order);
            }

            this.Hide();
            Frm_mainPage frm_mainPage = new Frm_mainPage(user);
            frm_mainPage.ShowDialog();
            this.Close();
        }

        private void insertIntoDatabase()
        {
            try
            {
                dao.OpenConnection();
                OleDbCommand comanda = new OleDbCommand();
                comanda.Connection = dao.getConnection();

                //adauagre comanda
                comanda.CommandText = "INSERT INTO comanda (ID, UtilizatorID, Greutate, Volum, Status, LatitudineAdresa, LongitudineAdresa) VALUES(?,?,?,?,?,?,?)";
                comanda.Parameters.Add("ID", OleDbType.Integer).Value = order.Id;
                comanda.Parameters.Add("UtilizatorID", OleDbType.Char, 30).Value = order.IdUser;
                comanda.Parameters.Add("Greutate", OleDbType.Double, 30).Value = order.Weight;
                comanda.Parameters.Add("Volum", OleDbType.Double, 30).Value = order.Volume;
                comanda.Parameters.Add("Status", OleDbType.Char, 30).Value = order.Status;
                comanda.Parameters.Add("LatitudineAdresa", OleDbType.Double, 30).Value = order.Address.Item1;
                comanda.Parameters.Add("LongitudineAdresa", OleDbType.Double, 30).Value = order.Address.Item2;
                comanda.ExecuteNonQuery();

                //adaugare rand_comanda
                List<Product> products = new List<Product>();
                List<int> noProducts = new List<int>();
                products = order.Products;
                noProducts = order.NoProducts;

                OleDbCommand comanda2 = new OleDbCommand();
                comanda2.Connection = dao.getConnection();

                foreach (var tuple in products.Zip(noProducts, (x, y) => (product: x, noProducts: y)))
                {
                    comanda2.CommandText = "SELECT MAX(ID) FROM rand_comanda";
                    int id = Convert.ToInt32(comanda2.ExecuteScalar());

                    comanda2.CommandText = "INSERT INTO rand_comanda (ID, ProdusID, ComandaID, Cantitate) VALUES(?,?,?,?)";
                    comanda2.Parameters.Add("ID", OleDbType.Integer).Value = id + 1;
                    comanda2.Parameters.Add("ProdusID", OleDbType.Integer).Value = tuple.product.Id;
                    comanda2.Parameters.Add("ComandaID", OleDbType.Integer).Value = order.Id;
                    comanda2.Parameters.Add("Cantitate", OleDbType.Integer).Value = tuple.noProducts;
                    comanda2.ExecuteNonQuery();
                }

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

        private void updateDatabase()
        {

        }
    }
}
