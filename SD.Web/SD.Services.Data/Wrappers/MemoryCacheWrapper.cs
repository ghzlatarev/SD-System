using Microsoft.Extensions.Caching.Memory;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Wrappers
{
	public class MemoryCacheWrapper : IMemoryCacheWrapper
	{
		private readonly IMemoryCache memCache;
		private readonly ISensorService sensorService;

		public MemoryCacheWrapper(IMemoryCache memoryCache, ISensorService sensorService)
		{
			this.memCache = memoryCache;
			this.sensorService = sensorService;
		}

		public async Task<IEnumerable<Sensor>> GetOrSetCache()
		{
			if (this.memCache.TryGetValue("ListOfSensors", out IEnumerable<Sensor> allSensors) == false)
			{
				allSensors = await this.sensorService.ListSensorsAsync(); //await this.dataContext.Sensors.ToListAsync();

				MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
				};

				this.memCache.Set("ListOfSensors", allSensors, options);
			}

			return allSensors;
		}
	}
}
