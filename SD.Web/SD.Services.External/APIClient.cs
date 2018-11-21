using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SD.Data.Models;
using SD.Data.Models.Abstract;

namespace SD.Services.External
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient client;

        public ApiClient(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri("http://telerikacademy.icb.bg/api/sensor/");
            this.client.DefaultRequestHeaders.Add("auth-token", "8e4c46fe-5e1d-4382-b7fc-19541f7bf3b0");
        }

        public async Task<IEnumerable<T>> GetEntities<T>(string target)
            where T : BaseEntity
        {
            var response = await client.GetAsync(target)
                .ConfigureAwait(false);
            
            var entities = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());

            return entities;
        }

        public async Task<T> GetSensorData<T>(string target)
            where T : BaseEntity
        {
            var response = await client.GetAsync(target)
                .ConfigureAwait(false);

            var entity = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

            return entity;
        }
    }
}
