using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
	public interface ISensorDataService
    {
        Task GetSensorsDataAsync();
    }
}
