using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
    public interface ISensorDataService
    {
        Task GetSensorsData();
        Task<Sensor> GetSensorsByIdAsync(string id);

    }
}
