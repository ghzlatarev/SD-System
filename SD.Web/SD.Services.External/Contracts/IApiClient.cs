using System.Collections.Generic;
using System.Threading.Tasks;
using SD.Data.Models.DomainModels;
using SD.Data.Models.Abstract;

namespace SD.Services.External
{
    public interface IApiClient
    {
        Task<IEnumerable<Sensor>> GetEntities(string target);

        Task<SensorData> GetSensorData(string target);
    }
}
