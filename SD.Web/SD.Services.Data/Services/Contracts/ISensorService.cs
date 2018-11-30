using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
    public interface ISensorService
    {
        Task RebaseSensorsAsync();

		Task<IList<Tuple<string, string>>> GetSensorNamesIdsAsync();
	}
}
