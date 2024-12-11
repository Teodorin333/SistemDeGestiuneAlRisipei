using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data; //for connectionState

namespace SistemDeGestiuneAlRisipei
{
    class DatabaseAccessObject
    {
        private readonly string connectionString;
        private OleDbConnection connection;

        public DatabaseAccessObject()
        {
            connectionString = @"Provider = Microsoft.ACE.OLEDB.16.0; Data Source = E:/Facultate/Licenta/BazaDeDate/SistemGestiuneDB.accdb;";
            connection = new OleDbConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public OleDbConnection getConnection()
        {
            return connection;
        }
    }
}
