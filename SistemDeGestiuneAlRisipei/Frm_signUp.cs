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

namespace SistemDeGestiuneAlRisipei
{
    public partial class Frm_signUp : KryptonForm
    {
        public Frm_signUp()
        {
            InitializeComponent();
        }

        private void Frm_signUp_Load(object sender, EventArgs e)
        {

        }

        private void rb_company_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rb_beneficiary_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            //save data from form and send it to the next form
            string email = tb_email.Text;
            string username = tb_username.Text;
            string phoneNumber = tb_phone.Text;
            string password = tb_password.Text;

            AUser user;

            if (rb_company.Checked)
            {
                user = new Company(email, username, password, phoneNumber);
                this.Hide();
                Frm_Address frm_Address = new Frm_Address(user);
                frm_Address.ShowDialog();
                this.Close();
            }
            if (rb_beneficiary.Checked)
            {
                user = new Beneficiary(email, username, password, phoneNumber);
                this.Hide();
                Frm_Address frm_Address = new Frm_Address(user);
                frm_Address.ShowDialog();
                this.Close();
            }
        }
    }
}
