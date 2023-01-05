using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;
using VuelosAPI_WPF.Models;
using VuelosAPI_WPF.Services;

namespace VuelosAPI_WPF.ViewModels
{
    internal class VuelosViewModel : INotifyPropertyChanged
    {
        Dispatcher dis;
        public ObservableCollection<Vuelo> Vuelos { get; set; } = new();
        public string VueloSeleccionado { get; set; } = "";
        private static System.Timers.Timer timer = new();
        private static System.Timers.Timer timerCancel = new();

        List<Vuelo> cancelados = new();
        public VuelosViewModel()
        {
            dis = Dispatcher.CurrentDispatcher;
            timer.Interval = 30000;
            timer.Elapsed += Actualizar;
            timer.Start();
            timerCancel.Interval = 40000;
            timerCancel.Elapsed += Eliminar;
            CargarVuelos();
        }

        async private void Eliminar(object? sender, ElapsedEventArgs e)
        {
            await service.Delete(cancelados.FirstOrDefault());
            cancelados.Clear();
            if (cancelados.Count == 0)
                timerCancel.Stop();
        }

        private void Actualizar(object? source, ElapsedEventArgs e)
        {
            dis.Invoke(() =>
            {
                ActualizarTabla();
            });
        }
        
        readonly VuelosService service = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        async void ActualizarTabla()
        {
            ObservableCollection<Vuelo> Comparador = new ObservableCollection<Vuelo>();
            var datos = await service.GetVuelos();
            datos.ForEach(x => Comparador.Add(x));
            if (Comparador.Count == Vuelos.Count)
            {
                for (int i = 0; i < Comparador.Count; i++)
                {
                    if (Comparador[i].Hora != Vuelos[i].Hora || Comparador[i].Puerta != Vuelos[i].Puerta
                        || Comparador[i].Destino != Vuelos[i].Destino || Comparador[i].Estado != Vuelos[i].Estado)
                    {
                        Vuelos.Clear();
                        datos.ForEach(x => Vuelos.Add(x));
                        break;
                    }
                }
            }
            else
            {
                datos.ForEach(x => Vuelos.Add(x));
            }
            foreach (var item in Vuelos)
            {
                int diference = (int)(TimeSpan.Parse(item.Hora).TotalMinutes - DateTime.Now.TimeOfDay.TotalMinutes);
                if (item.Estado != "CANCELADO" && item.Estado != "DESPEGANDO")
                {
                    if (diference <= 10 && diference>0)
                    {
                        item.Estado = "ABORDANDO";
                        await service.Update(item);
                    }
                    else if (diference > 10 && diference < 40)
                    {
                        item.Estado = "A TIEMPO";
                        await service.Update(item);
                    }
                    else if (diference > 40)
                    {
                        item.Estado = "PROXIMO EN LLEGAR";
                        await service.Update(item);
                    }
                    else if (diference <= 0)
                    {
                        item.Estado = "DESPEGANDO";
                        await service.Update(item);
                    }
                    else
                    {
                        item.Estado = "CANCELADO";
                        await service.Update(item);
                    }

                }
                else
                {
                    if (cancelados.Count() == 0)
                        cancelados.Add(item);
                    if (!timerCancel.Enabled)
                        timerCancel.Start();
                }
                
            }
            Vuelos.Clear();
            datos = await service.GetVuelos();
            datos.ForEach(x => Vuelos.Add(x));
            Actualizar(nameof(Vuelos));
        }
        async void CargarVuelos()
        {
            Vuelos.Clear();
            var datos = await service.GetVuelos();
            datos.ForEach(x => Vuelos.Add(x));
            Actualizar(nameof(Vuelos));
            ActualizarTabla();
        }
        public void Actualizar(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
