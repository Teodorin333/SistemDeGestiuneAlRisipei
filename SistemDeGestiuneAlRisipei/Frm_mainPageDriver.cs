using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using CefSharp;
using CefSharp.WinForms;

namespace SistemDeGestiuneAlRisipei
{
    public partial class Frm_mainPageDriver : KryptonForm
    {
        private Driver driver;

        private ChromiumWebBrowser browser;

        List<List<double>> list = new List<List<double>>
        {
            new List<double> { 44.40774300343208, 26.2064380533546 },
            new List<double> { 44.383947984343045, 26.11559358381572 },
            new List<double> { 44.42155923560085, 26.10276241631563 },
            new List<double> { 44.38974149242236, 26.108814851768702 },
            new List<double> { 44.43454094003413, 26.103793597493866 },
            new List<double> { 44.41887350073681, 26.05927991877774 },
            new List<double> { 44.428009511176946, 26.04311031792951 }
        };

        public Frm_mainPageDriver()
        {
            InitializeComponent();

            Cef.Initialize(new CefSettings());
        }

        public Frm_mainPageDriver(Driver driver) : this()
        {
            this.driver = driver;
        }

        private void Frm_mainPageDriver_Load(object sender, EventArgs e)
        {
            lbl_2.Text = driver.Username;
            loadRoutes();
        }

        private void loadRoutes()
        {
            List<Route> routesList = driver.ListRoutes;
            lv_routes.Items.Clear();

            foreach (Route route in routesList)
            {
                ListViewItem item = new ListViewItem(route.Id.ToString());
                item.SubItems.Add(route.TotalLength.ToString());
                item.SubItems.Add(route.RouteStatus.ToString());

                lv_routes.Items.Add(item);
            }

            lv_routes.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv_routes.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_login frm_login = new Frm_login();
            frm_login.ShowDialog();
            this.Close();
        }

        private void renderMap(List<List<double>> list)
        {
            List<string> colors = new List<string>();
            colors.AddRange(new List<string>
                {
                    "red",
                    "orange",
                    "yellow",
                    "green",
                    "blue",
                    "purple",
                    "pink",
                    "violet",
                    "black",
                    "white"
                });

            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    " +
                "<style>\r\n        #map {\r\n            " +
                "height: 400px;\r\n            width: 600px;\r\n        }\r\n   " +
                " </style>\r\n</head>\r\n<body>\r\n    <div id=\"map\"></div>\r\n    " +
                "<script>\r\n\r\nvar map;\r\nfunction initMap() {\r\n    var origin = {\r\n       " +
                " lat: 44.40930277855032,\r\n        lng: 26.113336187653346\r\n    };");


            for (int i = 1; i < list.Count - 1; i++)
            {
                var item = list[i];

                string punct = string.Format("\r\n    var punct{0} = {{\r\n   " +
                                             " lat: {1},\r\n     lng: {2}\r\n    }};",
                                             i, item[0], item[1]);
                sb.Append(punct);
            }

            sb.Append("\r\n\r\n\r\n    var settings = {\r\n        center: origin,\r\n        zoom: 10,\r\n        scaleControl: true\r\n    };\r\n\r\n    map = new google.maps.Map(document.getElementById('map'), settings);\r\n\r\n    let directionsService = new google.maps.DirectionsService();\r\n");

            for (int i = 1; i < list.Count - 2; i++)
            {
                string renderer = string.Format("let directionsRenderer{0}_{1} = new google.maps.DirectionsRenderer({{\r\n               " +
                    " suppressMarkers: true,\r\n                " +
                    "polylineOptions: {{\r\n                    " +
                    "strokeColor: '{2}',\r\n" +
                    "strokeWeight: 5,\r\n" +
                    "strokeOpacity: 0.6\r\n                }}\r\n            }});\r\n           " +
                    " directionsRenderer{0}_{1}.setMap(map);\r\n", i, i + 1, colors[i]);

                sb.Append(renderer);
            }

            sb.Append("\r\nlet directionsRendererD_0 = new google.maps.DirectionsRenderer({\r\n" +
                "   suppressMarkers: true,\r\n" +
                "   polylineOptions: {\r\n" +
                "   strokeColor: 'turqoise',\r\n" +
                "   strokeWeight: 5,\r\n" +
                "   strokeOpacity: 0.6\r\n}\r\n });\r\n" +
                "directionsRendererD_0.setMap(map);\r\n");

            sb.Append("\r\nlet directionsRendererLast_D = new google.maps.DirectionsRenderer({\r\n" +
                "suppressMarkers: true,\r\n" +
                "polylineOptions: {" +
                "strokeColor: 'cyan',\r\n" +
                "strokeWeight: 5,\r\n" +
                "strokeOpacity: 0.6\r\n}\r\n });\r\n" +
                "directionsRendererLast_D.setMap(map);\r\n");

            for (int i = 1; i < list.Count - 2; i++)
            {
                string route = string.Format("\r\nvar route{0}_{1}={{\r\n" +
                    "   origin: {2},\r\n" +
                    "   destination: {3},\r\n" +
                    "   travelMode: 'DRIVING'\r\n}}\r\n", i, i + 1, "punct" + i.ToString(), "punct" + (i + 1).ToString());

                sb.Append(route);
            }

            sb.Append("\r\nvar routeD_0={\r\n" +
                "origin: origin,\r\n" +
                "destination: punct1,\r\n" +
                "travelMode: 'DRIVING'\r\n}\r\n");

            string lastRoute = string.Format("\r\nvar routeLast_D={{\r\n" +
                "origin: {0},\r\n" +
                "destination: origin,\r\n" +
                "travelMode: 'DRIVING'\r\n}}\r\n", "punct" + (list.Count - 2).ToString());
            sb.Append(lastRoute);

            for (int i = 1; i < list.Count - 1; i++)
            {
                string marker = string.Format("\r\nnew google.maps.Marker({{\r\n" +
                    "   position: punct{0},\r\n" +
                    "   map: map,\r\n" +
                    "   label: '{0}'\r\n}});\r\n", i);

                sb.Append(marker);
            }

            sb.Append("\r\nnew google.maps.Marker({\r\n" +
                "position: origin,\r\n" +
                "map: map,\r\n" +
                "label: 'D'\r\n});\r\n");

            for (int i = 1; i < list.Count - 2; i++)
            {
                string direction = string.Format("\r\ndirectionsService.route(route{0}_{1}, function (response, status) {{\r\n" +
                    "if (status !== 'OK') {{\r\nwindow.alert('Directions request failed due to ' + status);\r\n return;\r\n " +
                    "}} else {{\r\ndirectionsRenderer{0}_{1}.setDirections(response);\r\n}}\r\n}}); \r\n  ", i, i + 1);

                sb.Append(direction);
            }

            sb.Append("\r\ndirectionsService.route(routeD_0, function(response, status){\r\n" +
                "if(status!=='OK'){\r\nwindow.alert('Directions request failed due to ' + status);\r\n return;\r\n" +
                "} else {\r\ndirectionsRendererD_0.setDirections(response);\r\n}\r\n}); \r\n");

            string lastDirection = string.Format("\r\ndirectionsService.route(routeLast_D, function (response, status) {{\r\n" +
                    "if (status !== 'OK') {{\r\nwindow.alert('Directions request failed due to ' + status);\r\n return;\r\n " +
                    "}} else {{\r\ndirectionsRendererLast_D.setDirections(response);\r\n}}\r\n}}); \r\n  ");
            sb.Append(lastDirection);


            sb.Append("}\r\n    </script>\r\n\r\n    <script async defer\r\n            src=\"https://maps.googleapis.com/maps/api/js?key=#########&callback=initMap\">\r\n    </script>\r\n</body>\r\n</html>");

            StreamWriter sw = new StreamWriter("E:/Facultate/Licenta/SistemDeGestiuneAlRisipei/DriverMap.html");
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        private void lv_routes_ItemActivate(object sender, EventArgs e)
        {
            
            Route selectedRoute = new Route();

            foreach (ListViewItem item in lv_routes.Items)
            {
                if (item.Selected)
                {
                    int routeId = Int32.Parse(item.SubItems[0].Text);

                    foreach(Route route in driver.ListRoutes)
                    {
                        if(route.Id == routeId)
                        {
                            selectedRoute = route;
                        }
                    }
                }
            }

            List<List<double>> selectedList = new List<List<double>>();

            foreach(Tuple<double, double> stop in selectedRoute.ListStops)
            {
                selectedList.Add(new List<double> {stop.Item1, stop.Item2});
            }

            renderMap(selectedList);

            string filePath = @"E:/Facultate/Licenta/SistemDeGestiuneAlRisipei/DriverMap.html";
            if (File.Exists(filePath))
            {
                panel1.Controls.Remove(browser);

                browser = new ChromiumWebBrowser(new Uri(filePath).AbsoluteUri);
                browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
                browser.JavascriptObjectRepository.Register("JavaScriptReader",
                    new JavaScriptReader(this), isAsync: false);

                browser.Dock = DockStyle.Fill;

                panel1.Controls.Add(browser);
            }
            else
            {
                MessageBox.Show("HTML file not found!");
            }
        }
    }
}
