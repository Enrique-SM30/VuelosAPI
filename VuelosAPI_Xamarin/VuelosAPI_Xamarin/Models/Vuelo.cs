using System;
using System.Collections.Generic;
using System.Text;

namespace VuelosAPI_Xamarin.Models
{
    public class Vuelo
    {
        public string Codigo { get; set; } = "";
        public string Hora { get; set; } = "";
        public string Destino { get; set; } = "";
        public string Puerta { get; set; } = "";
        public string Estado { get; set; } = "";
    }
}
