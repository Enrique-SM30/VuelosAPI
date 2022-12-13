using System;
using System.Collections.Generic;

namespace VuelosAPI.Models
{
    public partial class Vuelo
    {
        public string Codigo { get; set; } = null!;
        public string Hora { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public string Puerta { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
