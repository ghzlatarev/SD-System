using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Wrappers.Contracts
{
	public interface IMemoryCacheWrapper
	{
		Task<IEnumerable<Sensor>> GetOrSetCache();
	}
}
