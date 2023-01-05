using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VuelosAPI_WPF.Models;

namespace VuelosAPI_WPF.Services
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
        public async Task<List<Vuelo>> GetVuelos()
        {
            List<Vuelo>? vuelos = null;

            var response = await client.GetAsync("api/Vuelos");

            if (response.IsSuccessStatusCode) //status= 200 ok
            {
                var json = await response.Content.ReadAsStringAsync();
                vuelos = JsonConvert.DeserializeObject<List<Vuelo>?>(json);
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

        public async Task<bool> Update(Vuelo p)
        {
            var json = JsonConvert.SerializeObject(p);
            var response = await client.PutAsync("api/Vuelos", new StringContent(json, Encoding.UTF8,
                "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) //BadRequest
            {
                return false;
            }
            return true;
        }
        public async Task<bool> Delete(Vuelo p)
        {
            var response = await client.DeleteAsync("api/Vuelos/" + p.Codigo);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) //BadRequest
            {
                return false;
            }
            return true;
        }
    }
}
