using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemDeGestiuneAlRisipei
{
    public class Address
    {
        private int id;
        private double latitudineAdresa;
        private double longitudineAdresa;

        public Address()
        {
            this.id = 0;
            this.latitudineAdresa = 0;
            this.longitudineAdresa = 0;
        }

        public Address(int id, double latitudineAdresa, double longitudineAdresa)
        {
            this.id = id;
            this.latitudineAdresa = latitudineAdresa;
            this.longitudineAdresa = longitudineAdresa;
        }

        public int Id { get => id; set => id = value; }
        public double LatitudineAdresa { get => latitudineAdresa; set => latitudineAdresa = value; }
        public double LongitudineAdresa { get => longitudineAdresa; set => longitudineAdresa = value; }
    }
}
