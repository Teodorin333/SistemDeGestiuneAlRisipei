using System;

namespace SistemDeGestiuneAlRisipei
{
    public class JavaScriptReader
    {
        private Frm_Address frm;
        private Frm_mainPageDriver frm2;

        public JavaScriptReader(Frm_mainPageDriver form)
        {
            this.frm2 = form;
            Console.WriteLine("Am creat JavaReader");
        }

        public JavaScriptReader(Frm_Address form)
        {
            this.frm = form;
            Console.WriteLine("Am creat JavaReader");
        }

        public void receiveCoordinates(double lat, double lng, string location)
        {
            Console.WriteLine("Am apelat ReceiveCoordinates");
            Console.WriteLine(location);
            frm.UpdateCoordinates(lat, lng, location);
        }


    }
}
