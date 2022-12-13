using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VuelosAPI_WPF.Models
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
