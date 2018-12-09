using SD.Data.Models.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
	public interface ISensorService
    {
        Task RebaseSensorsAsync();

        Task<IEnumerable<Sensor>> ListSensorsAsync();

		Task<IEnumerable<Sensor>> ListStateSensorsAsync();

		Task<IEnumerable<Sensor>> ListNonStateSensorsAsync();

		Task<Sensor> GetSensorByIdAsync(string sensorId);
	}
}
