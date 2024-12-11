using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemDeGestiuneAlRisipei
{
    public class Product
    {
        private int id;
        private string name;
        private double weight;
        private double volume;
        private ProductCategoryEnum category;
        private int noItems; //cate bucati exista in stoc

        private static int idCounter = 180000;

        public Product()
        {
            //idCounter++;
            //this.id = idCounter;
        }

        public Product(int id, string name, double weight, double volume, ProductCategoryEnum category, int noItems)
        {
            idCounter++;
            this.id = id;
            this.name = name;
            this.weight = weight;
            this.volume = volume;
            this.category = category;
            this.noItems = noItems;
        }

        public Product(string name, double weight, double volume, ProductCategoryEnum category, int noItems)
        {
            idCounter++;
            this.id = idCounter;
            this.name = name;
            this.weight = weight;
            this.volume = volume;
            this.category = category;
            this.noItems = noItems;
        }

        public int Id { get => id; set => id = value; }

        public string Name { get => name; set => name = value; }

        public double Weight { get => weight; set => weight = value; }

        public double Volume { get => volume; set => volume = value; }

        public ProductCategoryEnum Category { get => category; set => category = value; }

        public int NoItems { get => noItems; set => noItems = value; }

        public void addToStock(int noItems)
        {
            this.noItems += noItems;
        }

        public bool pullFromStock(int noItems)
        {
            if (this.noItems < noItems) return false;

            this.noItems -= noItems;

            return true;
        }
    }
}
