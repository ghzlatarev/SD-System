using System.Collections.Generic;
using System.Threading.Tasks;
using SD.Data.Models.Abstract;

namespace SD.Services.External
{
    public interface IApiClient
    {
        Task<IEnumerable<T>> GetEntities<T>(string target)
            where T : BaseEntity;

        Task<T> GetSensorData<T>(string target)
            where T : BaseEntity;
    }
}
