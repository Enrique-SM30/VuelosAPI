using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VuelosAPI_Xamarin.Models;

namespace VuelosAPI_Xamarin.Services
{
    public class VuelosService
    {

        HttpClient client;

        public VuelosService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://aerolineaeg.sistemas19.com/")
            };
        }
        public event Action<List<string>> Error;
        public async Task<List<Vuelo>> GetVuelos()
        {
            List<Vuelo> vuelos = null;

            var response = await client.GetAsync("api/Vuelos");

            if (response.IsSuccessStatusCode) //status= 200 ok
            {
                var json = await response.Content.ReadAsStringAsync();
                vuelos = JsonConvert.DeserializeObject<List<Vuelo>>(json);
            }

            if (vuelos != null)
            {
                return vuelos;
            }
            else
            {
                return new List<Vuelo>();
            }
        }
        public async Task<bool> Insert(Vuelo p)
        {
            var json = JsonConvert.SerializeObject(p);
            var response = await client.PostAsync("api/Vuelos", new StringContent(json, Encoding.UTF8,
                "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
                return false;
            }
            return true;
        }

        public async Task<bool> Update(Vuelo p)
        {

            var json = JsonConvert.SerializeObject(p);
            var response = await client.PutAsync("api/Vuelos", new StringContent(json, Encoding.UTF8,
                "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) //BadRequest
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
                return false;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                LanzarError("Vuelo no encontrado");
            }
            return true;
        }

        public async Task<bool> Delete(Vuelo p)
        {
            var response = await client.DeleteAsync("api/Vuelos/" + p.Codigo);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) //BadRequest
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
                return false;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                LanzarError("Vuelo no encontrado");
            }
            return true;
        }

        void LanzarError(string mensaje)
        {
            Error?.Invoke(new List<string> { mensaje });
        }
        void LanzarErrorJson(string json)
        {
            List<string> obj = JsonConvert.DeserializeObject<List<string>>(json);
            if (obj != null)
            {
                Error?.Invoke(obj);
            }
        }
    }
}
