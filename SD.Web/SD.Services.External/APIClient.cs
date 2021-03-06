﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SD.Data.Models.DomainModels;
using SD.Data.Models.Abstract;
using Microsoft.AspNetCore.Http;
using System.Net;

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

        public async Task<IEnumerable<Sensor>> GetApiSensors(string target = "all")
        {
            var response = await client.GetAsync(target)
                .ConfigureAwait(false);
			
            var entities = JsonConvert.DeserializeObject<IEnumerable<Sensor>>(await response.Content.ReadAsStringAsync());

            return entities;
        }

        public async Task<SensorData> GetSensorData(string target)
        {
            var response = await client.GetAsync(target)
                .ConfigureAwait(false);
			
			if (response.StatusCode == HttpStatusCode.OK)
			{
				var entity = JsonConvert.DeserializeObject<SensorData>(await response.Content.ReadAsStringAsync());
				return entity;
			}
			else
			{
				return null;
			}	
        }
    }
}
