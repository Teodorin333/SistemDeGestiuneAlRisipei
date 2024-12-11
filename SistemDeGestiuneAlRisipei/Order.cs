using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemDeGestiuneAlRisipei
{
    public class Order
    {
        private int id;
        private int idUser;
        private Tuple<double, double> address;
        private List<Product> products;
        private List<int> noProducts;
        private double weight;
        private double volume;
        private OrderStatusEnum status;

        private static int idCounter = 170000;

        public Order()
        {
            idCounter++;
            this.id = idCounter;
            this.products = new List<Product>();
            this.noProducts = new List<int>();
            this.weight = 0;
            this.volume = 0;
            this.status = OrderStatusEnum.UNVERIFIED;
        }

        public Order(int id, int idUser, double weight, double volume, OrderStatusEnum status, double latitudineAdresa, double longitudineAdresa)
        {
            idCounter++;
            this.id = id;
            this.idUser = idUser;
            this.weight = weight;
            this.volume = volume;
            this.status = status;
            address = new Tuple<double, double>(latitudineAdresa, longitudineAdresa);

            this.products = new List<Product>();
            this.noProducts = new List<int>();
        }

        public Order(int idUser, List<Product> products, OrderStatusEnum status)
        {
            idCounter++;
            this.id = idCounter;
            this.idUser = idUser;
            this.products = products;
            this.weight = 0;
            this.volume = 0;
            this.status = status;
        }

        public Order(int idUser)
        {
            idCounter++;
            this.id = idCounter;
            this.idUser = idUser;
            this.products = new List<Product>();
            this.noProducts = new List<int>();
            this.weight = 0;
            this.volume = 0;
            this.status = OrderStatusEnum.UNVERIFIED;
        }

        public int Id { get => id; set => id = value; }
        public int IdUser { get => idUser; set => idUser = value; }
        public Tuple<double, double> Address { get => address; set => address = value; }
        public List<Product> Products { get => products; set => products = value; }
        public List<int> NoProducts { get => noProducts; set => noProducts = value; }
        public double Weight { get => weight; set => weight = value; }
        public double Volume { get => volume; set => volume = value; }
        public OrderStatusEnum Status { get => status; set => status = value; }

        public void addProduct(Product prod, int noItems)
        {
            this.products.Add(prod);
            this.noProducts.Add(noItems);
        }

        public void removeProduct(Product prod)
        {
            int index = this.Products.IndexOf(prod);
            this.noProducts.RemoveAt(index);
            this.products.Remove(prod);

        }
        public void removeProduct(int index)
        {
            this.noProducts.RemoveAt(index);
            this.products.RemoveAt(index);
        }
    }
}
