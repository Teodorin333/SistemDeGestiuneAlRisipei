using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Data.OleDb;

namespace SistemDeGestiuneAlRisipei
{
    public class Administrator
    {
        private int id;
        private string email;
        private string username;
        private string password;
        private string phoneNumber; //do we need this?
        private double salary;

        private static int idCounter = 100000;

        public Administrator()
        {
            idCounter++;
            this.id = idCounter;
        }

        public Administrator(int id, string email, string username, string password, string phoneNumber, double salary)
        {
            idCounter++;
            this.id = id;
            this.email = email;
            this.username = username;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.salary = salary;
        }

        public Administrator(string email, string username, string password, string phoneNumber, double salary)
        {
            idCounter++;
            this.id = idCounter;
            this.email = email;
            this.username = username;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.salary = salary;
        }
        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public double Salary { get => salary; set => salary = value; }

        public Route calculateRoute(List<Order> orders)
        {
            DatabaseAccessObject dao;
            dao = new DatabaseAccessObject();

            List<Order> ordersC = new List<Order>();
            List<Order> ordersB = new List<Order>();

            List<List<double>> list = new List<List<double>>();

            List<List<int>> cond = new List<List<int>>();

            int cnt = 0;

            foreach (Order order in orders)
            {
                if(order.IdUser < 140000)
                {
                    ordersC.Add(order);
                }
                else
                {
                    ordersB.Add(order);
                }
            }

            if (ordersC.Count > 0 && ordersB.Count > 0)
            {
                foreach (Order orderB in ordersB)
                {
                    List<Product> productsB = new List<Product>();
                    productsB = orderB.Products;

                    foreach (Order orderC in ordersC)
                    {
                        List<Product> productsC = new List<Product>();
                        productsC = orderC.Products;

                        foreach (Product productB in productsB)
                        {
                            foreach (Product productC in productsC)
                            {
                                if (productB.Id == productC.Id && cnt <6)
                                {
                                    Tuple<double, double> addressB = orderB.Address;
                                    Tuple<double, double> addressC = orderC.Address;

                                    list.Add(new List<double> { addressC.Item1, addressC.Item2 });
                                    list.Add(new List<double> { addressB.Item1, addressB.Item2 });

                                    cond.Add(new List<int> { cnt + 1, cnt + 2 });

                                    orderB.Status = OrderStatusEnum.IN_PICKUP;
                                    orderC.Status = OrderStatusEnum.IN_PICKUP;

                                    updateDatabase(dao, orderB.Id);
                                    updateDatabase(dao, orderC.Id);

                                    //add changes to database

                                    cnt += 2;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("nu avem");
                return null;
            }

            string pythonExe = @"C:\Users\xxxme\AppData\Local\Microsoft\WindowsApps\python.exe";
            string scriptPath = @"E:\Facultate\Licenta\slaygorithm\routeCalculator.py";

            string listJson = JsonSerializer.Serialize(list);
            string condJson = JsonSerializer.Serialize(cond);

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = $"{scriptPath} \"{listJson}\" \"{condJson}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(scriptPath)
            };
            
            string result = String.Empty;
            using (Process process = new Process())
            {
                process.StartInfo = start;

                process.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        result += args.Data + Environment.NewLine;
                    }
                };

                process.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        result += "Error: " + args.Data + Environment.NewLine;
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }

            
            // Print the raw output to verify its structure
            Console.WriteLine("Raw output from Python script:");
            Console.WriteLine(result);

            // Deserialize the result
            string cleanedResult = result.Trim();
            char[] separators = new char[] { ' ', '(', ')', '[', ']', ',' };
            string[] values = cleanedResult.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            double distance = Math.Round(double.Parse(values[values.Length - 1]), 2);
            Console.WriteLine(distance);

            List<int> nodes = new List<int>();

            for (var i = 0; i < values.Length - 1; i++)
            {
                nodes.Add(int.Parse(values[i]));
            }

            Route newRoute = new Route();

            Tuple<double, double> centerAddress = new Tuple<double, double>(44.4093027785503, 26.1133361876533);

            newRoute.addAddress(centerAddress);

            foreach (var index in nodes)
            {
                if(index < cnt)
                {
                    List<double> element = list.ElementAt(index);
                    Tuple<double, double> address = new Tuple<double, double>(element.ElementAt(0), element.ElementAt(1));
                    newRoute.addAddress(address);
                }
            }

            newRoute.addAddress(centerAddress);

            newRoute.TotalLength = distance;
            Console.WriteLine("Nr de opriri: " + newRoute.ListStops.Count.ToString());

            return newRoute;
        }

        private void updateDatabase(DatabaseAccessObject dao, int id)
        {
            try
            {
                dao.OpenConnection();
                OleDbCommand comanda = new OleDbCommand();
                comanda.Connection = dao.getConnection();
                comanda.CommandText = "UPDATE comanda SET status='" + "IN_PICKUP" + "' WHERE ID=" + id;
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

        public void assignRoute(Route route, Driver driver)
        {
            //soferul primeste ruta
        }
    }
}
