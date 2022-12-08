using System;
using System.Collections.Generic;

namespace VuelosAPI.Models
{
    public partial class Vuelo
    {
        public string Codigo { get; set; } = null!;
        public TimeOnly Hora { get; set; }
        public string Destino { get; set; } = null!;
        public string Puerta { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
