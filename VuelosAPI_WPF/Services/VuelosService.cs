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
                BaseAddress = new Uri("https://localhost:44388/")
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
    }
}
