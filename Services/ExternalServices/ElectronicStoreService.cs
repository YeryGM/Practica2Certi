using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Models;
using Serilog;

namespace Services.ExternalServices
{
    public class ElectronicStoreService
    {


        public async Task<List<Electronic>> GetAllElectronicItems()
        {
            try
            {
                string url = "https://api.restful-api.dev/objects";
                Log.Information("Me voy a conectar al siguiente HOST: " + url);

                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                List<Electronic> electronicItems = JsonConvert.DeserializeObject<List<Electronic>>(responseBody);
                return electronicItems;
            }
            catch (Exception ex)
            {
                Log.Error("Hubo un error de coneccion: " + ex.Message);
                throw ex;
            }
        }
    }
}