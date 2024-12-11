using System;
using System.IO;
using Microsoft.Win32;
using CefSharp;
using CefSharp.WinForms;
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
    public partial class Frm_Address : KryptonForm
    {
        private AUser user;

        private ChromiumWebBrowser browser;
        private double latitude;
        private double longitude;
        private string address;
        public Frm_Address()
        {
            InitializeComponent();

            Cef.Initialize(new CefSettings());

            this.Size = new Size(1300, 700);


            string filePath = @"E:\Facultate\Licenta\SistemDeGestiuneAlRisipei\SistemDeGestiuneAlRisipei\AddressFillScript.html";
            if (File.Exists(filePath))
            {
                browser = new ChromiumWebBrowser(new Uri(filePath).AbsoluteUri);

                browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
                browser.JavascriptObjectRepository.Register("JavaScriptReader",
                    new JavaScriptReader(this), isAsync: false);

                browser.Dock = DockStyle.Fill;

                this.Controls.Add(browser);
            }
            else
            {
                MessageBox.Show("HTML file not found!");
            }
        }

        public Frm_Address(AUser user) : this()
        {
            this.user = user;
        }

        private void Frm_Address_Load(object sender, EventArgs e)
        {
            
        }

        public void UpdateCoordinates(double lat, double lng, string location)
        {
            latitude = lat;
            longitude = lng;
            address = location;

            this.user.LatitudineAdresa = latitude;
            this.user.LongitudineAdresa = longitude;

            //Console.WriteLine("Am ajuns aici");
            //Console.WriteLine(latitude.ToString());
            //Console.WriteLine(longitude.ToString());
            //add address to the new account and send it to the next form
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_accountAdded frm_accountAdded = new Frm_accountAdded(this.user);
            frm_accountAdded.ShowDialog();
            this.Close();
        }
    }
}
