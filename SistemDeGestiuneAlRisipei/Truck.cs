using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemDeGestiuneAlRisipei
{
    public class Truck
    {
        private int id;
        private double maxWeight;
        private double maxVolume;
        private double fuel; //trebuie sa stim care e distanta maxima pe care o poate parcurge un camion

        private bool isInUse; //false -> la depozit, true -> utilizat pe o ruta

        private static int idCounter = 150000;

        public Truck()
        {
            idCounter++;
            this.id = idCounter;
        }

        public Truck(double maxWeight, double maxVolume, double fuel, bool isInUse)
        {
            idCounter++;
            this.id = idCounter;
            this.maxWeight = maxWeight;
            this.maxVolume = maxVolume;
            this.fuel = fuel;
            this.isInUse = isInUse;
        }
        public int Id { get => id; set => id = value; }
        public double MaxWeight { get => maxWeight; set => maxWeight = value; }
        public double MaxVolume { get => maxVolume; set => maxVolume = value; }
        public double Fuel { get => fuel; set => fuel = value; }
        public bool IsInUse { get => isInUse; set => isInUse = value; }


        //functie umple rezervor?
    }
}
