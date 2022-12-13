using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VuelosAPI_WPF.Models;
using VuelosAPI_WPF.Services;

namespace VuelosAPI_WPF.ViewModels
{
    internal class VuelosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Vuelo> Vuelos { get; set; } = new();
        public string VueloSeleccionado { get; set; } = "";
        public VuelosViewModel()
        {
            CargarVuelos();
        }
        readonly VuelosService service = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        async void CargarVuelos()
        {
            Vuelos.Clear();
            var datos = await service.GetVuelos();
            datos.ForEach(x => Vuelos.Add(x));
        }
    }
}
