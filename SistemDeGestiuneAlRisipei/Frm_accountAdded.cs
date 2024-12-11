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
    public partial class Frm_accountAdded : KryptonForm
    {
        private AUser user;
        private DatabaseAccessObject dao;
        public Frm_accountAdded()
        {
            InitializeComponent();
            dao = new DatabaseAccessObject();
        }

        public Frm_accountAdded(AUser user) : this()
        {
            this.user = user;
        }

        private void Frm_accountAdded_Load(object sender, EventArgs e)
        {

        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            saveData();
            this.Hide();
            Frm_login frm_Login = new Frm_login();
            frm_Login.ShowDialog();
            this.Close();
        }

        private void saveData()
        {
            if(user.Username != null)
            {
                Program.users.Add(user);
                try
                {
                    dao.OpenConnection();
                    OleDbCommand comanda = new OleDbCommand();
                    comanda.Connection = dao.getConnection();

                    comanda.CommandText = "INSERT INTO utilizator (ID, Email, Username, Parola, LatitudineAdresa, LongitudineAdresa, Telefon) VALUES(?,?,?,?,?,?,?)";
                    comanda.Parameters.Add("ID", OleDbType.Integer).Value = user.Id;
                    comanda.Parameters.Add("Email", OleDbType.Char, 30).Value = user.Email;
                    comanda.Parameters.Add("Username", OleDbType.Char, 30).Value = user.Username;
                    comanda.Parameters.Add("Parola", OleDbType.Char, 30).Value = user.Password;
                    comanda.Parameters.Add("LatitudineAdresa", OleDbType.Double, 30).Value = user.LatitudineAdresa;
                    comanda.Parameters.Add("LongitudineAdresa", OleDbType.Double, 30).Value = user.LongitudineAdresa;
                    comanda.Parameters.Add("Telefon", OleDbType.Char, 30).Value = user.PhoneNumber;
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
}
