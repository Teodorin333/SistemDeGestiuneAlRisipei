using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemDeGestiuneAlRisipei
{
    public class Route
    {
        private int id;
        private double totalLength; //nr de km acoperiti de ruta
        private bool isAssigned; //daca ruta a fost sau nu asignata unui sofer
        private int idDriver;

        private List<Tuple<double, double>> listStops; //lista cu toate punctele prin care trebuie sa treaca ruta, in ordine, retinute ca perechi de coordonate lat si long
        //putem retine eventual si nr de km dintre fiecare punct consecutiv

        private RouteStatusEnum routeStatus; //statusul curent al rutei
        //calculata -> atunci cand este creata
        //atribuita -> a fost predata unui sofer
        //in parcurgere -> atunci cand soferul porneste pe ruta?
        //completa -> cand s-a ajuns la punctul final al rutei

        //mereu primul si ultimul punct vor fi un depozit

        private static int idCounter = 160000;

        public Route()
        {
            idCounter++;
            this.id = idCounter;
            this.routeStatus = RouteStatusEnum.UNASSIGNED;
            listStops = new List<Tuple<double, double>>();
        }

        public Route(int id, double totalLength, bool isAssigned, RouteStatusEnum routeStatus, int idDriver)
        {
            idCounter++;
            this.id = id;
            this.totalLength = totalLength;
            this.isAssigned = isAssigned;
            this.routeStatus = routeStatus;
            listStops = new List<Tuple<double, double>>();
            this.idDriver = idDriver;
        }

        public Route(int id, double totalLength, bool isAssigned, RouteStatusEnum routeStatus)
        {
            idCounter++;
            this.id = id;
            this.totalLength = totalLength;
            this.isAssigned = isAssigned;
            this.routeStatus = routeStatus;
            listStops = new List<Tuple<double, double>>();
            this.idDriver = 0;
        }

        public Route(double totalLength, List<Tuple<double, double>> listStops)
        {
            idCounter++;
            this.id = idCounter;
            this.totalLength = totalLength;
            this.listStops = listStops;
            this.routeStatus = RouteStatusEnum.UNASSIGNED;
        }

        public int Id { get => id; set => id = value; }
        public int IdDriver { get => idDriver; set => idDriver = value; }
        public double TotalLength { get => totalLength; set => totalLength = value; }
        public RouteStatusEnum RouteStatus { get => routeStatus; set => routeStatus = value; }
        public bool IsAssigned { get => isAssigned; set => isAssigned = value; }
        public List<Tuple<double, double>> ListStops { get => listStops; set => listStops = value; }

        public void addAddress(Address address)
        {
            //Tuple<double, double> address1 = new Tuple<double, double>(latitudineAdresa, longitudineAdresa);
            ListStops.Add(new Tuple<double, double>(address.LatitudineAdresa, address.LongitudineAdresa));
        }

        public void addAddress(Tuple<double, double> address)
        {
            //Tuple<double, double> address1 = new Tuple<double, double>(latitudineAdresa, longitudineAdresa);
            ListStops.Add(address);
        }

        override
        public string ToString()
        {
            return "Id ruta: " + this.id.ToString() + ", lungime totala: " + this.totalLength.ToString();
        }

    }
}
