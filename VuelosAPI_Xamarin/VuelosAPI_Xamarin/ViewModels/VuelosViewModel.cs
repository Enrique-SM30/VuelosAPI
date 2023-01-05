using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using VuelosAPI_Xamarin.Models;
using VuelosAPI_Xamarin.Services;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using TimePicker = Xamarin.Forms.TimePicker;

namespace VuelosAPI_Xamarin.ViewModels
{
    public enum Vistas { Ver, Agregar, Editar}
    public class VuelosViewModel : INotifyPropertyChanged
    {
        private static System.Timers.Timer timer = new System.Timers.Timer();
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Vuelo> Vuelos { get; set; } = new ObservableCollection<Vuelo>();
        public Vuelo Vuelo { get; set; } = new Vuelo();
        public TimeSpan Horaa { get; set; }
        public string Error { get; set; } = "";
        public ObservableCollection<string> Puertas { get; set; } = new ObservableCollection<string>();
        readonly VuelosService serviceVuelos = new VuelosService();
        

        public Vistas Vista { get; set; } = Vistas.Ver;
            
        public ICommand AgregarCommand { get; set; }
        public ICommand VolverCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VistaEditarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand EditarCommand { get; set; }

        public VuelosViewModel()
        { 
            Puertas.Add("A01");
            Puertas.Add("A02");
            Puertas.Add("A03");
            Puertas.Add("B01");
            Puertas.Add("B02");
            Puertas.Add("B03");
            Puertas.Add("C01");
            Puertas.Add("C02");
            Puertas.Add("C03");
            timer.Interval = 20000;
            timer.Elapsed += Actualizar;
            timer.Start();
            Vista = Vistas.Ver;
            serviceVuelos.Error += ServiceVuelos_Errors;
            AgregarCommand = new RelayCommand(Guardar);
            VistaEditarCommand = new RelayCommand<Vuelo>(VistaEditar);
            EditarCommand = new RelayCommand<bool>(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CambiarVistaCommand = new RelayCommand<Vistas>(CambiarVista);
            CancelarCommand=new RelayCommand<Vuelo>(Cancelar);
            VolverCommand = new RelayCommand<Vuelo>(Volver);
            CargarVuelos();
        }
        async private void Actualizar(object source, ElapsedEventArgs e)
        {
            if(Vista==Vistas.Ver)
            {
                ObservableCollection<Vuelo> Comparador = new ObservableCollection<Vuelo>();
                var datos = await serviceVuelos.GetVuelos();
                datos.ForEach(x => Comparador.Add(x));
                if (Comparador.Count == Vuelos.Count)
                {
                    for (int i = 0; i < Comparador.Count; i++)
                    {
                        if(Comparador[i].Hora != Vuelos[i].Hora || Comparador[i].Puerta != Vuelos[i].Puerta 
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
                Actualizar(nameof(Vuelos));
            }

        }
        private void Volver(Vuelo obj)
        {
            CambiarVista(Vistas.Ver);
        }
        async void CargarVuelos()
        {
            Vuelos.Clear();
            var datos = await serviceVuelos.GetVuelos();
            datos.ForEach(x => Vuelos.Add(x));
            Actualizar(nameof(Vuelos));
        }

        void CambiarVista(Vistas v)
        {
            Vista = v;
            Error = "";
            if (v == Vistas.Agregar)
            {
                Vuelo = new Vuelo();
            }

            Actualizar(nameof(Vista));
        }
        async void Guardar()
        {
            if (Vuelo != null)
            {
                if (!Vuelos.Contains(Vuelo))
                {
                    Vuelo.Hora = Horaa.ToString().Substring(0, 5);
                    Vuelo.Estado = Status();
                    if (await serviceVuelos.Insert(Vuelo))
                    {
                        CambiarVista(Vistas.Ver);
                    }
                }
                CargarVuelos();
            }
        }

        public string Status()
        {
            int diference = (int)(Horaa.TotalMinutes - DateTime.Now.TimeOfDay.TotalMinutes);
            if (diference < 10)
            {
                return "ABORDANDO";
            }
            else if (diference > 10 && diference<40)
            {
                return "A TIEMPO";
            }
            else if (diference > 40)
            {
                return "PROXIMO EN LLEGAR";
            }
            else if (diference < 0)
            {
                return "DESPEGANDO";
            }
            else
            {
                return "CANCELADO";
            }
        }

        void VistaEditar(Vuelo v)
        {
            Vuelo = new Vuelo
            {
                Codigo=v.Codigo,
                Puerta=v.Puerta,
                Hora=v.Hora,
                Destino=v.Destino,
                Estado=v.Estado
            };
            CambiarVista(Vistas.Editar);
        }
        async void Editar(bool Cancelado)
        {
            if (Vuelo != null)
            {

                Vuelo.Hora = Horaa.ToString().Substring(0, 5);
                if(Cancelado==false)
                Vuelo.Estado = Status();
                if (await serviceVuelos.Update(Vuelo))
                {
                    CargarVuelos();
                    CambiarVista(Vistas.Ver);
                }
            }
        }
        void Cancelar(Vuelo v)
        {
            Vuelo = new Vuelo
            {
                Codigo = v.Codigo,
                Puerta = v.Puerta,
                Hora = v.Hora,
                Destino = v.Destino,
                Estado = "CANCELADO"
            };
            Editar(true);
        }

        async void Eliminar()
        {
            if (Vuelo != null)
            {
                if (await serviceVuelos.Delete(Vuelo))
                {
                    CargarVuelos();
                    CambiarVista(Vistas.Ver);
                }
            }
        }

        private void ServiceVuelos_Errors(List<string> obj)
        {
            Error = string.Join("\n", obj);
            Actualizar(nameof(Error));
        }
        public void Actualizar(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
